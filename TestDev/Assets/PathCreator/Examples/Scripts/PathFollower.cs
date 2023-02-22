using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;
        public delegate void OnMoveDone();
        public event OnMoveDone Event_MoveDone;
        bool m_MoveDone;

        void Start()
        {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null && !m_MoveDone)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                if (transform.position == pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1))
                {
                    //END OF PATH
                    Debug.LogWarning("END PATH");
                    if (Event_MoveDone != null)
                        Event_MoveDone.Invoke();
                    if (endOfPathInstruction == EndOfPathInstruction.Stop)
                        m_MoveDone = true;
                }
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
        public void Func_SetMoveDone(bool _value)
        {
            m_MoveDone = _value;
        }
    }
}