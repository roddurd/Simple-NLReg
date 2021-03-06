﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace GradDesc
{
    class Program
    {        
        static List<double> Xs = new List<double>();
        static List<double> Ys = new List<double>();
        static double theta0 = 0, theta1 = 0, theta2 = 0, theta3=0, theta4=0, alpha = 0.01, errorVal=0;
        static int numIterations = 5000;
        
        static double error(double x, double y)
        { return hypothesis(x) - y; }
        
        static double hypothesis(double x)
        {return theta4 * Math.Pow(x, 4) + theta3 * Math.Pow(x, 3) + theta2 * Math.Pow(x, 2) + theta1 * x + theta0;}
        
        static void Main(string[] args)
        {
            Setup();
        }
        static void Setup()
        {            
            Console.ForegroundColor = ConsoleColor.DarkGray; // ~ a e s t h e t i c s ~
            Console.WriteLine("Write the file path to your data. Each line is considered an input/output pair.\nX and Y values must be separated by either a comma, space, OR tab (cannot be more than one).");
            Console.Write("\nExample Path: C:\\Users\\roddur.dasgupta\\Desktop\\data.txt\n\tPath: ");
            string path = Console.ReadLine();
            try
            {
                foreach (string line in File.ReadLines(path))
                {
                    if (line.Length > 0)
                    {
                        if (!line.Substring(0, 1).Equals("*")) //for comments
                        {
                            char[] del = { ',', ' ', '\t' };
                            string[] vals = line.Split(del);
                            Xs.Add(Convert.ToDouble(vals[0].Trim()));
                            Ys.Add(Convert.ToDouble(vals[1].Trim()));
                        }
                    }
                }
            }
            #region stupid user handling
            catch (FileNotFoundException fnfe)
            {
                Console.WriteLine("File not found. Please check your file path. Error details:");
                Console.WriteLine(fnfe.Message);
                Console.WriteLine("Press enter to try again.");
                Console.ReadLine();
                Console.Clear();
                Setup();
            }
            catch (FormatException fe)
            {
                Console.WriteLine("Oops! Looks like your data may not be formatted correctly.\nX and Y values must be separated by either a comma, space, OR tab (cannot be more than one).\nFull error details:");
                Console.WriteLine(fe.Message);
                Console.WriteLine("Press enter when you're ready to try again.");
                Console.ReadLine();
                Console.Clear();
                Setup();
            }
            catch (DirectoryNotFoundException dnfe)
            {
                Console.WriteLine("Oops! The directory was not found. Perhaps there is a typo in your file path?\nHere is the full error message: ");
                Console.WriteLine(dnfe.Message);
                Console.WriteLine("Press enter when you're ready to try again.");
                Console.ReadLine();
                Console.Clear();
                Setup();
            }
            catch (Exception e)
            {
                Console.WriteLine("Okay I'm usually good with exceptions but I have no idea what you messed up.\nHere, Microsoft will tell you what you did wrong:");
                Console.WriteLine(e.Message);
                Console.WriteLine("Press enter when you're ready to try again.");
                Console.ReadLine();
                Console.Clear();
                Setup();
            }
            
            Console.Write("Data successfully parsed.\nChoose a learning rate [You probably shouldn't exceed 0.01]: ");
            string response = Console.ReadLine();
            while (!double.TryParse(response, out alpha))
            {
                Console.Write("Input was not a double. Try again, choose a learning rate: ");
                response = Console.ReadLine();
            }
            if (alpha > 0.3)
            {
                Console.WriteLine("Ok, a little big, don't you think? Let's just stick with 0.01.");
                alpha = 0.01;
            }
            if (alpha < 0.0000001)
            {
                Console.WriteLine("Ok, a little small, don't you think? Let's just stick with 0.000001.");
                alpha = 0.000001;
            }
            Console.Write("\nLearning rate is {0}. Choose a # of iterations [For best results, a minimum of 5000 is recommended]: ", alpha);
            response = Console.ReadLine();
            while (!int.TryParse(response, out numIterations))
            {
                Console.Write("Input was not an integer. Try again, choose a number of iterations: ");
                response = Console.ReadLine();
            }
            if (numIterations < 500)
            {
                Console.WriteLine(numIterations + " is a pretty small number of iterations. Let's stick with 1000.");
                numIterations = 1000;                
            }
            if (numIterations > 1000000)
            {
                Console.WriteLine("Capping number of iterations at a million.");
                numIterations = 1000000;
            }
            #endregion
            Regression();       
        }

        static void Regression()
        {
            Console.WriteLine("Press enter to start gradient descent algorithm.");
            Console.ReadLine();

            double temp0 = 0, temp1 = 0, temp2 = 0, temp3 = 0, temp4 = 0;
            for (int i = 0; i < numIterations; i++)
            {
                errorVal = 0;
                for (int j = 0; j < Xs.Count; j++)
                {
                    
                    temp0 = theta0 - alpha * error(Xs[j], Ys[j]);
                    temp1 = theta1 - alpha * error(Xs[j], Ys[j]) * Xs[j] / Xs.Max();
                    temp2 = theta2 - alpha * error(Xs[j], Ys[j]) * Math.Pow(Xs[j],2) / Math.Pow(Xs.Max(), 2);
                    temp3 = theta3 - alpha * error(Xs[j], Ys[j]) * Math.Pow(Xs[j],3) / Math.Pow(Xs.Max(), 3);
                    temp4 = theta4 - alpha * error(Xs[j], Ys[j]) * Math.Pow(Xs[j],4) / Math.Pow(Xs.Max(), 4);
                    theta0 = temp0;
                    theta1 = temp1;
                    theta2 = temp2;
                    theta3 = temp3;
                    theta4 = temp4;
                    errorVal += error(Xs[j], Ys[j]);
                }
                errorVal /= Xs.Count;
                Console.WriteLine("theta0: {0}\ttheta1: {1}\ntheta2: {2}\ttheta3: {3}\ntheta4: {4}\terror: {5}", theta0, theta1, theta2, theta3, theta4, errorVal);              
            }
            string bestmodel = "Best model: ";
            theta4 = Math.Round(theta4);
            theta3 = Math.Round(theta3);
            theta2 = Math.Round(theta2);
            theta1 = Math.Round(theta1);
            theta0 = Math.Round(theta0);

            #region aesthetics
            switch (theta4)
            {
                case -1:
                    bestmodel += "-x^4";
                    break;
                case 1:
                    bestmodel += "x^4";
                    break;
                case 0:
                    break;
                default:
                    bestmodel += theta4 + "x^4";
                    break;
            }
            switch (theta3)
            {
                case -1:
                    bestmodel += "-x^3";
                    break;
                case 1:
                    if (theta4 == 0)
                        bestmodel += "x^3";
                    else
                        bestmodel += "+x^3";
                    break;
                case 0:
                    break;
                default:
                    if (theta4 == 0)
                        bestmodel += theta3 + "x^3";
                    else
                        bestmodel += theta3 > 0 ? "+" + theta3 + "x^3" : theta3 + "x^3";
                    break;
            }
            switch (theta2)
            {
                case -1:
                    bestmodel += "-x^2";
                    break;
                case 1:
                    if (theta3 == 0)
                        bestmodel += "x^2";
                    else
                        bestmodel += "+x^2";
                    break;
                case 0:
                    break;
                default:
                    if (theta3 == 0)
                        bestmodel += theta2 + "x^2";
                    else
                        bestmodel += theta2 > 0 ? "+" + theta2 + "x^2" : theta2 + "x^2";
                    break;
            }            
            switch (theta1)
            {
                case -1:
                    bestmodel += "-x";
                    break;
                case 1:
                    if (theta3==0 && theta2==0)
                        bestmodel += "x";
                    else
                        bestmodel += "+x";
                    break;
                case 0:
                    break;
                default:
                    if (theta3 == 0 && theta2 == 0)
                        bestmodel += theta1 + "x";
                    else
                        bestmodel += theta1 > 0 ? "+" + theta1 + "x" : theta1 + "x";
                    break;
            }
            if (theta0 > 0)
                bestmodel += "+" + theta0;
            else if (theta0 < 0)
                bestmodel += theta0;              
            #endregion

            Console.WriteLine(bestmodel);
            //error with rounded thetas:
            errorVal = 0;
            for (int i = 0; i < Xs.Count; i++)
            {
                errorVal += error(Xs[i], Ys[i]);
            }
            errorVal /= Xs.Count;
            Console.WriteLine("Average error: " + errorVal);
            Console.WriteLine("Press enter if you want to apply nonlinear regression to another data file!");
            Console.ReadLine();
            Setup();
        }   
    }
}
