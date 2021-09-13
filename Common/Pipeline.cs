using System.Numerics;

namespace Common
{
    public struct PersProjInfo
    {
        public float FOV;
        public float Width;
        public float Height;
        public float zNear;
        public float zFar;
    };

    public struct OrthoProjInfo
    {
        public float r;        // right
        public float l;        // left
        public float b;        // bottom
        public float t;        // top
        public float n;        // z near
        public float f;        // z far
    };

    public class Orientation
    {
        public Vector3 m_scale;
        public Vector3 m_rotation;
        public Vector3 m_pos;

        private Orientation()
        {
            m_scale = new Vector3(1.0f, 1.0f, 1.0f);
            m_rotation = new Vector3(0.0f, 0.0f, 0.0f);
            m_pos = new Vector3(0.0f, 0.0f, 0.0f);
        }
    };

    public class Pipeline
    {
        private Vector3 m_scale;
        private Vector3 m_worldPos;
        private Vector3 m_rotateInfo;

        private PersProjInfo m_persProjInfo;
        private OrthoProjInfo m_orthoProjInfo;

        private Matrix4x4 m_WVPtransformation;
        private Matrix4x4 m_VPtransformation;
        private Matrix4x4 m_WPtransformation;
        private Matrix4x4 m_WVtransformation;
        private Matrix4x4 m_Wtransformation;
        private Matrix4x4 m_Vtransformation;
        private Matrix4x4 m_ProjTransformation;

        public struct CameraPosition
        {
            public Vector3 Pos;
            public Vector3 Target;
            public Vector3 Up;
        }

        private CameraPosition m_camera;

        public Pipeline()
        {
            m_scale = new Vector3(1.0f, 1.0f, 1.0f);
            m_worldPos = new Vector3(0.0f, 0.0f, 0.0f);
            m_rotateInfo = new Vector3(0.0f, 0.0f, 0.0f);
        }

        public void Scale(float s)
        {
            Scale(s, s, s);
        }

        public void Scale(Vector3 scale)
        {
            Scale(scale.X, scale.Y, scale.Z);
        }

        public void Scale(float ScaleX, float ScaleY, float ScaleZ)
        {
            m_scale.X = ScaleX;
            m_scale.Y = ScaleY;
            m_scale.Z = ScaleZ;
        }

        public void WorldPos(float x, float y, float z)
        {
            m_worldPos.X = x;
            m_worldPos.Y = y;
            m_worldPos.Z = z;
        }

        public void WorldPos(Vector3 Pos)
        {
            m_worldPos = Pos;
        }

        public void Rotate(float RotateX, float RotateY, float RotateZ)
        {
            m_rotateInfo.X = RotateX;
            m_rotateInfo.Y = RotateY;
            m_rotateInfo.Z = RotateZ;
        }

        public void Rotate(Vector3 r)
        {
            Rotate(r.X, r.Y, r.Z);
        }

        public void SetPerspectiveProj(PersProjInfo p)
        {
            m_persProjInfo = p;
        }

        public void SetOrthographicProj(OrthoProjInfo p)
        {
            m_orthoProjInfo = p;
        }

        public void SetCamera(Vector3 Pos, Vector3 Target, Vector3 Up)
        {
            m_camera.Pos = Pos;
            m_camera.Target = Target;
            m_camera.Up = Up;
        }

        private void SetCamera(Camera camera)
        {
            SetCamera(camera.GetPos(), camera.GetTarget(), camera.GetUp());
        }

        private void Orient(Orientation o)
        {
            m_scale = o.m_scale;
            m_worldPos = o.m_pos;
            m_rotateInfo = o.m_rotation;
        }

        public Matrix4x4 GetProjTrans()
        {
            m_ProjTransformation = Matrix4x4.CreatePerspective(
                m_persProjInfo.Width,
                m_persProjInfo.Height,
                m_persProjInfo.zNear,
                m_persProjInfo.zFar);
            return m_ProjTransformation;
        }

        public Matrix4x4 GetVPTrans()
        {
            GetViewTrans();
            GetProjTrans();

            m_VPtransformation = m_ProjTransformation * m_Vtransformation;
            return m_VPtransformation;
        }

        public Matrix4x4 GetWorldTrans()
        {
            Matrix4x4 ScaleTrans = Matrix4x4.CreateScale(m_scale.X, m_scale.Y, m_scale.Z);
            Matrix4x4 RotateTrans = Matrix4x4.CreateFromYawPitchRoll(m_rotateInfo.X, m_rotateInfo.Y, m_rotateInfo.Z);
            Matrix4x4 TranslationTrans = Matrix4x4.CreateTranslation(m_worldPos.X, m_worldPos.Y, m_worldPos.Z);

            m_Wtransformation = TranslationTrans * RotateTrans * ScaleTrans;
            return m_Wtransformation;
        }

        public Matrix4x4 GetViewTrans()
        {
            m_Vtransformation = Matrix4x4.CreateLookAt(m_camera.Pos, m_camera.Target, m_camera.Up);

            return m_Vtransformation;
        }

        public Matrix4x4 GetWVPTrans()
        {
            GetWorldTrans();
            GetVPTrans();

            m_WVPtransformation = m_VPtransformation * m_Wtransformation;
            return m_WVPtransformation;
        }

        public Matrix4x4 GetWVOrthoPTrans()
        {
            GetWorldTrans();
            GetViewTrans();

            Matrix4x4 P = Matrix4x4.CreateOrthographicOffCenter(
                m_orthoProjInfo.l,
                m_orthoProjInfo.r,
                m_orthoProjInfo.b,
                m_orthoProjInfo.t,
                m_orthoProjInfo.n,
                m_orthoProjInfo.f);

            m_WVPtransformation = P * m_Vtransformation * m_Wtransformation;
            return m_WVPtransformation;
        }

        public Matrix4x4 GetWVTrans()
        {
            GetWorldTrans();
            GetViewTrans();

            m_WVtransformation = m_Vtransformation * m_Wtransformation;
            return m_WVtransformation;
        }

        public Matrix4x4 GetWPTrans()
        {
            Matrix4x4 PersProjTrans = Matrix4x4.CreatePerspective(
                m_persProjInfo.Width,
                m_persProjInfo.Height,
                m_persProjInfo.zNear,
                m_persProjInfo.zFar);

            GetWorldTrans();

            m_WPtransformation = PersProjTrans * m_Wtransformation;
            return m_WPtransformation;
        }
    };
}