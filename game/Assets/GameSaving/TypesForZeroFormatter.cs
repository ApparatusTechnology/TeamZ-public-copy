#if INCLUDE_ONLY_CODE_GENERATION

namespace UnityEngine
{
    public struct Vector2
    {
        [Index(0)]
        public float x;
        [Index(1)]
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct Vector3
    {
        [Index(0)]
        public float x;

        [Index(1)]
        public float y;

        [Index(2)]
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public struct Quaternion
    {
        [Index(0)]
        public float x;
        [Index(1)]
        public float y;
        [Index(2)]
        public float z;
        [Index(3)]
        public float w;

        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}

#endif