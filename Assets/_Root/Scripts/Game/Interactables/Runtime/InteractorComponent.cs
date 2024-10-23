using _Root.Scripts.Game.FocusProvider.Runtime;
using Sirenix.OdinInspector;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractorComponent : MonoBehaviour, IInteractor
    {
        [SerializeField] private bool addInteractableAsParentAndHide = true;

        [FormerlySerializedAs("playerOverlap")]
        public OverlapCheckedNonAlloc playerOverlapChecked;

        private IFocusConsumer _focusConsumerReference;
        public IntervalTicker ticker;
        private IInteractable _lastInteractable;
        public bool busy;
        public Collider _lastClosestCollider;


        private void Awake() => _focusConsumerReference = GetComponent<IFocusConsumer>();

        private void Start()
        {
            playerOverlapChecked.Initialize();
        }

        private void Update()
        {
            if (busy) return;
            if (playerOverlapChecked.TryGetClosest(out var closestCollider, out _))
            {
                if (_lastClosestCollider == null) SetupInteractable(closestCollider);
                else if (_lastClosestCollider != closestCollider)
                {
                    _lastInteractable.OnHoverExit(this);
                    SetupInteractable(closestCollider);
                }
            }
            else if (_lastClosestCollider != null)
            {
                _lastInteractable.OnHoverExit(this);
                _lastClosestCollider = null;
                _lastInteractable = null;
            }
        }

        private void SetupInteractable(Collider closestCollider)
        {
            _lastClosestCollider = closestCollider;
            var parentOrSelf = closestCollider.transform.parent ?? closestCollider.transform;
            _lastInteractable = parentOrSelf.GetComponent<IInteractable>();
            _lastInteractable.OnInteractHoverEnter(this);
        }

        public GameObject GameObject => gameObject;

        public void OnInteractStart(IInteractable interactable)
        {
            if (addInteractableAsParentAndHide)
            {
                transform.SetParent(interactable.GameObject.transform);
                GameObject.SetActive(false);
            }
        }

        public void OnInteractEnd(IInteractable interactable)
        {
            if (addInteractableAsParentAndHide)
            {
                transform.SetParent(interactable.GameObject.transform);
                GameObject.SetActive(false);
            }
        }


        [Button]
        public void Interact()
        {
            if (_lastInteractable == null) return;
            busy = true;
            _lastInteractable.OnInteractStart(this);
        }

        private void FixedUpdate()
        {
            if (!busy && ticker.TryTick()) playerOverlapChecked.Perform();
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            playerOverlapChecked.DrawGizmos(Color.red, Color.green);
        }
#endif
        public bool IsFocused => _focusConsumerReference.IsFocused;
    }
}