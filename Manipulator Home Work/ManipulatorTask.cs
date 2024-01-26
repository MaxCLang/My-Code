using System;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        /// <summary>
        /// Возвращает массив углов (shoulder, elbow, wrist),
        /// необходимых для приведения эффектора манипулятора в точку x и y 
        /// с углом между последним суставом и горизонталью, равному alpha (в радианах)
        /// См. чертеж manipulator.png!
        /// </summary>
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            // Используйте поля Forearm, UpperArm, Palm класса Manipulator
            double wristX = x + Manipulator.Palm * Math.Cos(-alpha-Math.PI);
            double wristY = y + Manipulator.Palm * Math.Sin(-alpha-Math.PI);
            double length = Math.Sqrt(wristX*wristX + wristY*wristY);
            double elbowAngle = TriangleTask.GetABAngle(Manipulator.UpperArm,Manipulator.Forearm,length);
            double partShoulderAngle = TriangleTask.GetABAngle(Manipulator.UpperArm, length, Manipulator.Forearm);
            double secondPartShoulderAngle = Math.Atan2(wristY, wristX);
            double shoulderAngle = partShoulderAngle + secondPartShoulderAngle;
            double wristAngle = - alpha - shoulderAngle - elbowAngle;
            if (shoulderAngle == double.NaN || elbowAngle == double.NaN || wristAngle == double.NaN)
                return new[] { double.NaN, double.NaN, double.NaN };
            return new[] { shoulderAngle, elbowAngle, wristAngle };
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        [Test]
        public void TestMoveManipulatorTo()
        {
            Random random = new Random();
            for (int i = 0; i < 1; i++)
            {
                double x = random.Next(-200, 200);
                double y = random.Next(-200, 200);
                double d = random.NextDouble();
                double two = random.Next(0, 1);
                double pi;
                if (two == 0) pi = 2*Math.PI;
                else pi = -2*Math.PI;
                double alpha = pi * d;
                double sh = ManipulatorTask.MoveManipulatorTo(x, y, alpha)[0];
                double el = ManipulatorTask.MoveManipulatorTo(x, y, alpha)[1];
                double wr = ManipulatorTask.MoveManipulatorTo(x, y, alpha)[2];
                double px = AnglesToCoordinatesTask.GetJointPositions(sh, el, wr)[2].X;
                double py = AnglesToCoordinatesTask.GetJointPositions(sh, el, wr)[2].Y;
                Assert.AreEqual(y, py, 1e-5);
                Assert.AreEqual(x, px,1e-5);
            }

            //Assert.Fail("Write randomized test here!");
        }
    }
}