﻿using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOptimization
{
    public class TestRunner
    {
        public static void RunTests()
        {
            string reportFilePath = "raport.csv";

            List<TestResult> testResults = new List<TestResult>();

            foreach (var testFunction in TestingFunctions.Functions)
            {
                foreach (var nValue in Main.N)
                {
                    foreach (var tValue in Main.I)
                    {
                        SnakeOptimization snakeOptimization = new SnakeOptimization(
                            _N: nValue,
                            _T: tValue,
                            _funkcja: testFunction.Funkcja,
                            _dim: testFunction.Dim,
                            _xmin: testFunction.Xmin,
                            _xmax: testFunction.Xmax
                        );

                        for (int i = 0; i < Main.n; i++)
                        {
                            double[] food_position;
                            double bestFitValue;
                            int iFobj;

                            (food_position, bestFitValue, iFobj) = snakeOptimization.Solve();

                            TestResult testResult = new TestResult
                            {
                                TestFunctionName = testFunction.Funkcja.Method.Name,
                                NumberOfParameters = testFunction.Dim,
                                NumberOfIterations = tValue,
                                PopulationSize = nValue,
                                FoundMinimum = bestFitValue,
                                StdDevParameters = 0, // TODO odchylenie standardowe parametrów
                                ObjectiveValue = 0, // TODO wartość funkcji celu
                                StdDevObjectiveValue = 0, // TODO odchylenie standardowe wartości funkcji celu
                            };

                            // wynik na liście
                            testResults.Add(testResult);
                        }
                    }
                }
            }

            // Zapisz CSV
            var config = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
            using (var writer = new StreamWriter(reportFilePath))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(testResults);
            }
        }
    }
}

