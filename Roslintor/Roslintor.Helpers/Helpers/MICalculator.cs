namespace Roslintor.Helpers.Helpers
{
    public static class MICalculator
    {
        public static double CalculateMI(double halsteadVolume, int cyclomaticComplexity, int linesOfCode)
        {
            double A = 171;
            double B = 5.2;
            double C = 0.23;
            double D = 16.2;

            return A - B * System.Math.Log(halsteadVolume) - C * cyclomaticComplexity - D * System.Math.Log(linesOfCode);
        }
    }
}
