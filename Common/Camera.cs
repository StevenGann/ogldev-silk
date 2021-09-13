using System;
using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.Input;

namespace Common
{
    public class Camera
    {
        private Vector3 m_pos;
        private Vector3 m_target;
        private Vector3 m_up;

        private int m_windowWidth;
        private int m_windowHeight;

        private float m_AngleH;
        private float m_AngleV;

        private bool m_OnUpperEdge;
        private bool m_OnLowerEdge;
        private bool m_OnLeftEdge;
        private bool m_OnRightEdge;

        private Vector2D<int> m_mousePos;

        private const float STEP_SCALE = 1.0f;
        private const float EDGE_STEP = 0.5f;
        private const int MARGIN = 10;

        public Camera(int WindowWidth, int WindowHeight)
        {
            m_windowWidth = WindowWidth;
            m_windowHeight = WindowHeight;
            m_pos = new Vector3(0.0f, 0.0f, 0.0f);
            m_target = new Vector3(0.0f, 0.0f, 1.0f);
            //m_target.Normalize();
            m_up = new Vector3(0.0f, 1.0f, 0.0f);

            Init();
        }

        public Camera(int WindowWidth, int WindowHeight, Vector3 Pos, Vector3 Target, Vector3 Up)
        {
            m_windowWidth = WindowWidth;
            m_windowHeight = WindowHeight;
            m_pos = Pos;

            m_target = Vector3.Normalize(Target);

            m_up = Vector3.Normalize(Up);

            Init();
        }

        public bool OnKeyboard(Key Key)
        {
            bool Ret = false;

            switch (Key)
            {
                case Key.Up:
                    {
                        m_pos += (m_target * STEP_SCALE);
                        Ret = true;
                    }
                    break;

                case Key.Down:
                    {
                        m_pos -= (m_target * STEP_SCALE);
                        Ret = true;
                    }
                    break;

                case Key.Left:
                    {
                        Vector3 Left = Vector3.Normalize(Vector3.Cross(m_target, m_up));
                        Left *= STEP_SCALE;
                        m_pos += Left;
                        Ret = true;
                    }
                    break;

                case Key.Right:
                    {
                        Vector3 Right = Vector3.Normalize(Vector3.Cross(m_up, m_target));
                        Right *= STEP_SCALE;
                        m_pos += Right;
                        Ret = true;
                    }
                    break;

                case Key.PageUp:
                    m_pos.Y += STEP_SCALE;
                    break;

                case Key.PageDown:
                    m_pos.Y -= STEP_SCALE;
                    break;

                default:
                    break;
            }

            return Ret;
        }

        public void OnMouse(int X, int Y)
        {
            int DeltaX = X - m_mousePos.X;
            int DeltaY = Y - m_mousePos.Y;

            m_mousePos.X = X;
            m_mousePos.Y = Y;

            m_AngleH += DeltaX / 20.0f;
            m_AngleV += DeltaY / 20.0f;

            if (DeltaX == 0)
            {
                if (X <= MARGIN)
                {
                    //    m_AngleH -= 1.0f;
                    m_OnLeftEdge = true;
                }
                else if (X >= (m_windowWidth - MARGIN))
                {
                    //    m_AngleH += 1.0f;
                    m_OnRightEdge = true;
                }
            }
            else
            {
                m_OnLeftEdge = false;
                m_OnRightEdge = false;
            }

            if (DeltaY == 0)
            {
                if (Y <= MARGIN)
                {
                    m_OnUpperEdge = true;
                }
                else if (Y >= (m_windowHeight - MARGIN))
                {
                    m_OnLowerEdge = true;
                }
            }
            else
            {
                m_OnUpperEdge = false;
                m_OnLowerEdge = false;
            }

            Update();
        }

        public void OnRender()
        {
            bool ShouldUpdate = false;

            if (m_OnLeftEdge)
            {
                m_AngleH -= EDGE_STEP;
                ShouldUpdate = true;
            }
            else if (m_OnRightEdge)
            {
                m_AngleH += EDGE_STEP;
                ShouldUpdate = true;
            }

            if (m_OnUpperEdge)
            {
                if (m_AngleV > -90.0f)
                {
                    m_AngleV -= EDGE_STEP;
                    ShouldUpdate = true;
                }
            }
            else if (m_OnLowerEdge)
            {
                if (m_AngleV < 90.0f)
                {
                    m_AngleV += EDGE_STEP;
                    ShouldUpdate = true;
                }
            }

            if (ShouldUpdate)
            {
                Update();
            }
        }

        public Vector3 GetPos()
        {
            return m_pos;
        }

        public Vector3 GetTarget()
        {
            return m_target;
        }

        public Vector3 GetUp()
        {
            return m_up;
        }

        public void AddToATB()//out TwBar bar)
        {
        }

        private void Init()
        {
            Vector3 HTarget = Vector3.Normalize(new(m_target.X, 0.0f, m_target.Z));

            if (HTarget.Z >= 0.0f)
            {
                if (HTarget.X >= 0.0f)
                {
                    m_AngleH = 360.0f - ToDegree(MathF.Asin(HTarget.Z));
                }
                else
                {
                    m_AngleH = 180.0f + ToDegree(MathF.Asin(HTarget.Z));
                }
            }
            else
            {
                if (HTarget.X >= 0.0f)
                {
                    m_AngleH = ToDegree(MathF.Asin(-HTarget.Z));
                }
                else
                {
                    m_AngleH = 180.0f - ToDegree(MathF.Asin(-HTarget.Z));
                }
            }

            m_AngleV = -ToDegree(MathF.Asin(m_target.Y));

            m_OnUpperEdge = false;
            m_OnLowerEdge = false;
            m_OnLeftEdge = false;
            m_OnRightEdge = false;
            m_mousePos.X = m_windowWidth / 2;
            m_mousePos.Y = m_windowHeight / 2;
        }

        private void Update()
        {
        }

        private float ToRadian(float x)
        {
            return (float)(((x) * MathF.PI / 180.0f));
        }

        private float ToDegree(float x)
        {
            return (float)(((x) * 180.0f / MathF.PI));
        }
    }
}