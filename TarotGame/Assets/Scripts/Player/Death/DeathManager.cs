using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class DeathManager : MonoBehaviour
{
    [SerializeField]
    private GameObject deathPrefab;

    private GridTransform gridTransform;
    private PlayLoadedLevelV2 playLevel;
    private DeathMovement death = null;

    public UnityEvent onAllTorchesLit;

    private bool currentTargetIsDim = false;

    void Awake()
    {
        playLevel = FindObjectOfType<PlayLoadedLevelV2>();
        gridTransform = GetComponent<GridTransform>();

        playLevel.onLoadEnded.RemoveListener(OnLoadEnd);
        playLevel.onLoadEnded.AddListener(OnLoadEnd);

        FindObjectOfType<StateChange>().onLateStateChange.RemoveListener(FindTarget);
        FindObjectOfType<StateChange>().onLateStateChange.AddListener(FindTarget);
    }

    private void OnLoadEnd()
    {
        foreach(Torch t in Torch.torches)
        {
            t.onInteractionSuccess.RemoveListener(OnTorchLit);
            t.onInteractionSuccess.AddListener(OnTorchLit);
            t.onExtinguish.RemoveListener(OnTorchExtinguished);
            t.onExtinguish.AddListener(OnTorchExtinguished);
        }
    }

    private void FindTarget()
    {
        if (death == null) return;

        Vector3Int currentClosest = new(1000,1000,1000);

        Vector3Int deathPos = death.GetComponent<GridTransform>().GetGridPosition();

        if(Torch.brightTorches.Count > 0)
            foreach(Torch t in Torch.brightTorches)
            {
                Vector3Int tPos = t.GetComponent<GridTransform>().GetGridPosition();
                if ((currentClosest - deathPos).magnitude > (tPos - deathPos).magnitude)
                {
                    currentClosest = tPos;
                    currentTargetIsDim = false;
                }
            }
        else
            foreach (Torch t in Torch.dimTorches)
            {
                Vector3Int tPos = t.GetComponent<GridTransform>().GetGridPosition();
                if ((currentClosest - deathPos).magnitude > (tPos - deathPos).magnitude)
                {
                    currentClosest = tPos;
                    currentTargetIsDim = true;
                }
            }

        death.SetTarget(currentClosest);
    }

    private void OnTorchLit(Interactor interactor, Interactable interactable)
    {
        if(death == null)
        {
            //spawn death
            GameObject go = Instantiate(deathPrefab);
            death = go.GetComponent<DeathMovement>();
            death.GetComponent<GridTransform>().SetGridPosition(gridTransform.GetGridPosition(), 0);
            FindTarget();
            death.onTargetReached.RemoveListener(FindTarget);
            death.onTargetReached.AddListener(FindTarget);
            BillboardManager.Instance.UpdateSprites(Camera.main);
        }

        int litTorches = Torch.brightTorches.Count + Torch.dimTorches.Count;

        if(Torch.torches.Count == litTorches)
        {
            //despawn death
            Destroy(death.gameObject);
            death = null;

            //all torches lit
            onAllTorchesLit?.Invoke();
        }

        if (currentTargetIsDim)
            FindTarget();
    }

    private void OnTorchExtinguished()
    {
        int litTorches = Torch.brightTorches.Count + Torch.dimTorches.Count;
        if (litTorches == 0)
        {
            // despawn death on next move
            FindObjectOfType<PlayerMovement>().onChariotMove.RemoveListener(DespawnDeath);
            FindObjectOfType<PlayerMovement>().onPlayerMove.RemoveListener(DespawnDeath);
            FindObjectOfType<PlayerMovement>().onChariotMove.AddListener(DespawnDeath);
            FindObjectOfType<PlayerMovement>().onPlayerMove.AddListener(DespawnDeath);
        }
    }

    private void DespawnDeath()
    {
        FindObjectOfType<PlayerMovement>().onChariotMove.RemoveListener(DespawnDeath);
        FindObjectOfType<PlayerMovement>().onPlayerMove.RemoveListener(DespawnDeath);

        int litTorches = Torch.brightTorches.Count + Torch.dimTorches.Count;
        if (litTorches == 0)
        {
            //despawn death
            Destroy(death.gameObject);
            death = null;
        }
        else
        {
            FindTarget();
        }
    }
}
