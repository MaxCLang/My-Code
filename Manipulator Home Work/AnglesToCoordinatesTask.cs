using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var upperArm = Manipulator.UpperArm;
            var forearm = Manipulator.Forearm;
            var palm = Manipulator.Palm;
            var angle1 = shoulder + elbow - Math.PI;
            var angle2 = angle1 + wrist - Math.PI;

            var elbowCos = (float)(Math.Cos(shoulder) * upperArm);
            var elbowSin = (float)(Math.Sin(shoulder) * upperArm);
            var wristCos = (float)(elbowCos + Math.Cos(angle1) * forearm);
            var wristSin = (float)(elbowSin + Math.Sin(angle1) * forearm);
            var palmCos = (float)(wristCos + Math.Cos(angle2) * palm);
            var palmSin = (float)(wristSin + Math.Sin(angle2) * palm);

            var elbowPos = new PointF(elbowCos, elbowSin);
            var wristPos = new PointF(wristCos, wristSin);
            var palmEndPos = new PointF(palmCos, palmSin);
            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        // Доработайте эти тесты!
        // С помощью строчки TestCase можно добавлять новые тестовые данные.
        // Аргументы TestCase превратятся в аргументы метода.
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI / 2, Manipulator.Forearm, Manipulator.UpperArm - Manipulator.Palm)]
        [TestCase(Math.PI / 2, 3 * Math.PI / 2, 3 * Math.PI / 2, -Manipulator.Forearm, Manipulator.UpperArm - Manipulator.Palm)]
        [TestCase(Math.PI / 2, Math.PI, 3 * Math.PI, 0, Manipulator.Forearm + Manipulator.UpperArm + Manipulator.Palm)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
            Assert.AreEqual(GetDistance(joints[0], new PointF(0, 0)), Manipulator.UpperArm);
            Assert.AreEqual(GetDistance(joints[0], joints[1]), Manipulator.Forearm);
            Assert.AreEqual(GetDistance(joints[1], joints[2]), Manipulator.Palm);
        }

        public double GetDistance(PointF point1, PointF point2)
        {
            var differenceX = (point1.X - point2.X) * (point1.X - point2.X);
            var differenceY = (point1.Y - point2.Y) * (point1.Y - point2.Y);
            return Math.Sqrt(differenceX + differenceY);
        }
    }
}