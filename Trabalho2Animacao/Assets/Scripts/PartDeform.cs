using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Collections.Specialized;

namespace Deform
{

    [Deformer(Name = "Offset", Type = typeof(PartDeformer))]
    public class PartDeformer : Deformer
    {
        private float curFrame = 0;
        public GameObject Twist;
        public GameObject Bend;
        public GameObject Stretch;
        public Vector3 offset;
        public override DataFlags DataFlags => DataFlags.Vertices;

        public float speed = 5.0f;
        private const int stateIdle = 0;
        private const int stateTwist = 1;
        private const int stateBend = 2;
        private const int stateSquash = 3;
        private const int stateStretch = 4;
        private int curState = 0;

     
        private int bendDelta = 0;


        private bool squashing = false;
        private bool stretching = false;
        private bool squashingAgain = false;
        private int dir = 1;


        public GameObject particleA;
        public GameObject particleB;
        public GameObject particleC;

        private List<GameObject> particlesA;

        public override JobHandle Process(MeshData data, JobHandle dependency = default)
        {
           
            
            return new OffsetJob
            {
                offset = offset,
                vertices = data.DynamicNative.VertexBuffer
            }.Schedule(data.Length,128,dependency);
        }
        [BurstCompile]
        public struct OffsetJob : IJobParallelFor
        {
            public float3 offset;
            public NativeArray<float3> vertices;


            public void Execute(int index)
            {
                vertices[index] += offset;
            }
        }

        public void Start()
        {
            //Twist = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;

            curState = stateIdle;
            bendDelta = 0;
            dir = 1;
            squashing = false;
            stretching = false;
           squashingAgain = false;

            particlesA = new List<GameObject>();
    }

        public void Update()
        {
            if ((Input.GetKey("left") || Input.GetKey("a")) && curState == stateIdle) {

                curState = stateTwist;
                dir = 1;
                var delta = speed * Time.deltaTime;

                curFrame -=1;
                if (curFrame <= 0)
                {
                    curFrame = 360;
                    Twist.GetComponent<TwistDeformer>().StartAngle = 360;
                }
                else
                {
                Twist.GetComponent<TwistDeformer>().StartAngle = curFrame;
                } 
                
                this.transform.position -= new Vector3(delta,0,0);
            }else if ((Input.GetKey("right") || Input.GetKey("d")) && curState == stateIdle)
            {
                curState = stateTwist;
                dir = -1;
                var delta = speed * Time.deltaTime;
                curFrame += 1;
                if (curFrame >= 360)
                {
                    curFrame = 0;
                    Twist.GetComponent<TwistDeformer>().StartAngle = 0;
                }
                else
                {
                    Twist.GetComponent<TwistDeformer>().StartAngle = curFrame;
                }
                this.transform.position += new Vector3(delta,0,0);
            }else if (bendDelta == 1)
            {
                curState = stateBend;
                if (Bend.GetComponent<BendDeformer>().Angle <= 180) {
                    Bend.GetComponent<BendDeformer>().Angle++;
                    Bend.GetComponent<BendDeformer>().Axis.rotation = new Quaternion(Bend.GetComponent<BendDeformer>().Angle, 0, 0,0);
                }
                else
                {
                    bendDelta = -1;
                }
            }
            else if (bendDelta == -1)
            {
                curState = stateBend;
                if (Bend.GetComponent<BendDeformer>().Angle  >= 0)
                {
                    Bend.GetComponent<BendDeformer>().Angle--;
                    Bend.GetComponent<BendDeformer>().Axis.rotation = new Quaternion(Bend.GetComponent<BendDeformer>().Angle,0,0,0);
                }
                else
                {
                    bendDelta = 0;
                }
            }else if (Input.GetKey("w") && curState == stateIdle)
            {
                curState = stateSquash;

               
            }
            else if (curState == stateSquash)
            {
                if (!squashing)
                {
                    squashing = true;

                }
                else if (!stretching)
                {
                    Stretch.GetComponent<SquashAndStretchDeformer>().Factor -= 0.01f;
                    if (Stretch.GetComponent<SquashAndStretchDeformer>().Factor <= -1)
                    {
                        Stretch.GetComponent<SquashAndStretchDeformer>().Factor = -1;
                        squashing = false;
                        stretching = true;
                       
                    }
                }
                else if(!squashingAgain)
                {
                    Stretch.GetComponent<SquashAndStretchDeformer>().Factor += 0.01f;
                    if (Stretch.GetComponent<SquashAndStretchDeformer>().Factor >= 1)
                    {
                        Stretch.GetComponent<SquashAndStretchDeformer>().Factor = 1;
                        stretching = false;
                        squashingAgain = true;
                        
                    }
                }
                else
                {
                    Stretch.GetComponent<SquashAndStretchDeformer>().Factor -= 0.01f;
                    if (Stretch.GetComponent<SquashAndStretchDeformer>().Factor <= 0)
                    {
                        Stretch.GetComponent<SquashAndStretchDeformer>().Factor = 0;
                        squashingAgain = false;
                        curState = stateIdle;

                    }
                }
            }
            else
            {
                curState = stateIdle;
            }

            switch (curState)
            {
                case stateTwist:
                    GameObject newParticle;
                    newParticle= Instantiate(particleA,transform.position,Quaternion.identity);
                    particlesA.Add(newParticle);
                    break;
                case stateBend:
                    GameObject newParticleB;
                    newParticleB = Instantiate(particleB, transform.position, Quaternion.identity);
                    particlesA.Add(newParticleB);
                    break;
                case stateSquash:
                    GameObject newParticleC;
                    newParticleC = Instantiate(particleC, transform.position, Quaternion.identity);
                    particlesA.Add(newParticleC);
                    break;
                default:
                    break;
            }


            for (int i = 0; i < particlesA.Count; i++)
            {
                var p = particlesA[i];
                if (p.GetComponent<Particle>().ativa == false)
                {
                    DestroyImmediate(p,true);
                    particlesA.RemoveAt(i);
                }
            }
        }

        public void OnMouseDown()
        {
            if (bendDelta == 0)
            {
                curState = stateBend;
                bendDelta = 1;
            }
        }

        

    }
}
