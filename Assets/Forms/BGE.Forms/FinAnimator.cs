using UnityEngine;
using System.Collections;

namespace BGE.Forms
{
    public class FinAnimator : Animator {

        public enum Axis {X, Y, Z};
        public Axis axis = Axis.X;
        public enum Side { left, right , tail};
        public Side side = Side.left;
        private Harmonic harmonic;

        [Range(0.0f, Utilities.TWO_PI)]
        public float theta = 0;
        float initialAmplitude;
        public float amplitude = 40.0f;

        [Range(0, 360)]
        public float rotationOffset = 220;

        [Range(0, 8)]
        public float wigglyness = 1;
        // Use this for initialization
        void Start () {
            if (boid != null)
            {
                harmonic = boid.GetComponent<Harmonic>();
                if (harmonic != null)
                {
                    initialAmplitude = harmonic.amplitude;
                    theta = harmonic.theta;
                }
            }
        }

        [HideInInspector]
        public float lerpedAmplitude;
        // Update is called once per frame
        void Update () {        
            if (harmonic != null)
            {
                theta = harmonic.theta;
                float offset = rotationOffset * Mathf.Deg2Rad;

                // Are we Banking?
                if (((side == Side.left && boid.bank < -5) || (side == Side.right && boid.bank > 5)))
                {
                    lerpedAmplitude = Mathf.Lerp(lerpedAmplitude, 0, Time.deltaTime);
                }
                else
                {
                    lerpedAmplitude  = Mathf.Lerp(lerpedAmplitude, amplitude, Time.deltaTime);
                }
            
                float angle = Mathf.Sin((theta * wigglyness + offset))
                              * (harmonic.rampedAmplitude / initialAmplitude) * lerpedAmplitude;
                switch (axis)
                {
                    case Axis.X:
                        transform.localRotation = Quaternion.Euler(angle, 0, 0);
                        break;
                    case Axis.Y:
                        transform.localRotation = Quaternion.Euler(0, angle, 0);
                        break;
                    case Axis.Z:
                        transform.localRotation = Quaternion.Euler(0, 0, angle);
                        break;
                }
            }
        }
    }
}