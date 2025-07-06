using System;

class Program
{
    static void Main()
    {
        double D0 = 200;              // Начальный диаметр катушки (мм)
        double d = 20;                // Диаметр нити (мм)
        double L = 5000;              // Длина катушки (мм)
        double S = 200000;            // Общая длина нити (мм)
        double omega = 2;             // Скорость вращения катушки (оборотов в секунду)
        double lead = 2;              // мм - шаг ходового винта (lead)
        int stepsPerRevolution = 200; // кол-во шагов на оборот

        double speedDevice = omega * d;
        Console.WriteLine($"Скорость устройства: {speedDevice:F2} мм/с\n");

        SimulateWinding(D0, d, L, S, omega, lead, stepsPerRevolution);
    }

    static void SimulateWinding(double D0, double d, double L, double S, double omega, double lead, int stepsPerRevolution)
    {
        Console.WriteLine($"Диаметр нити: {d} мм");
        Console.WriteLine($"Шаг ходового винта устройства: {lead} мм");

        double linearSpeed = omega * d; // мм/с
        double motorRPM = linearSpeed * 60 / lead;

        Console.WriteLine($"Требуемая скорость устройства: {linearSpeed:F3} мм/с");
        Console.WriteLine($"Скорость мотора: {motorRPM:F3} об/мин");

        double pulseFrequency = (motorRPM * stepsPerRevolution) / 60;
        Console.WriteLine($"Частота импульсов для шагового мотора: {pulseFrequency:F1} Гц");

        int layer = 0;
        double accumulatedLength = 0;
        double totalTime = 0;

        int turnsPerFullLayer = (int)(L / d);

        Console.WriteLine("\n=== Процесс намотки ===");
        Console.WriteLine("|  № | Диаметр | Витков | Длина нити | Время слоя | Общее время |");
        Console.WriteLine("---------------------------------------------------------------");

        while (accumulatedLength < S)
        {
            double currentDiameter = D0 + 2 * layer * d;

            int maxTurnsForThisLayer = turnsPerFullLayer;

            double maxLengthThisLayer = maxTurnsForThisLayer * Math.PI * currentDiameter;

            double remainingLength = S - accumulatedLength;

            int actualTurns = maxTurnsForThisLayer;
            double actualLengthThisLayer = maxLengthThisLayer;

            if (maxLengthThisLayer > remainingLength)
            {
                actualLengthThisLayer = remainingLength;
                actualTurns = (int)(remainingLength / (Math.PI * currentDiameter));
            }

            double timeThisLayer = (double)actualTurns / omega;
            totalTime += timeThisLayer;

            accumulatedLength += actualLengthThisLayer;

            Console.WriteLine($"| {layer + 1,2} | {currentDiameter,7:F2} | {actualTurns,6} | {actualLengthThisLayer,10:F2} | {timeThisLayer,9:F2} | {totalTime,10:F2} |");

            layer++;
        }

        Console.WriteLine("---------------------------------------------------------------");
        Console.WriteLine($"\nОбщее время намотки: {totalTime:F2} секунд");
        Console.WriteLine($"Всего намотано слоёв: {layer}");
        Console.ReadKey();
    }
}