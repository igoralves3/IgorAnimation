using System.Collections;
//using System.Collections.Generic;
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
        public Vector3 offset;
        public override DataFlags DataFlags => DataFlags.Vertices;

        public float speed = 5.0f;
        private int stateIdle = 0;
        private int stateTwist = 1;
        private int stateBend = 2;
        private int curState = 0;

     
        private int bendDelta = 0;

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
        }

        public void Update()
        {
            if (Input.GetKey("left") || Input.GetKey("a")) {

                curState = stateTwist;

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
            }else if (Input.GetKey("right") || Input.GetKey("d"))
            {
                curState = stateTwist;
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
            }if (bendDelta == 1)
            {
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
                if (Bend.GetComponent<BendDeformer>().Angle  >= 0)
                {
                    Bend.GetComponent<BendDeformer>().Angle--;
                    Bend.GetComponent<BendDeformer>().Axis.rotation = new Quaternion(Bend.GetComponent<BendDeformer>().Angle,0,0,0);
                }
                else
                {
                    bendDelta = 0;
                }
            }
            

        }

        public void OnMouseDown()
        {
            if (bendDelta == 0)
            {
                bendDelta = 1;
            }
        }

    }
}
