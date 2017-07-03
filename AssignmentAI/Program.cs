using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentAI
{
    class Program
    {
        // Initialising the random number generator
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(min, max);
            }
        }

        static void Main(string[] args)
        {
            // Declaring the lists used to store the data

            // Following lists include all the data of the courses
            List<Ingredient> ingredientList = new List<Ingredient>();
            List<Course> courseList = new List<Course>();
            List<CourseRestriction> restrictionListBreakfastOne = new List<CourseRestriction>();
            List<CourseRestriction> restrictionListBreakfastTwo = new List<CourseRestriction>();
            List<CourseRestriction> restrictionListLunchOne = new List<CourseRestriction>();
            List<CourseRestriction> restrictionListLunchTwo = new List<CourseRestriction>();
            List<CourseRestriction> restrictionListLunchThree = new List<CourseRestriction>();
            List<CourseRestriction> restrictionListDinnerOne = new List<CourseRestriction>();
            List<CourseRestriction> restrictionListDinnerTwo = new List<CourseRestriction>();
            List<CourseRestriction> restrictionListDinnerThree = new List<CourseRestriction>();
            List<CourseRestriction> restrictionListDinnerFour = new List<CourseRestriction>();

            // Following lists are used to create the menus
            List<Day> dayList = new List<Day>();
            List<Week> weekListParent = new List<Week>();
            List<Week> weekListTemp = new List<Week>();
            List<Week> weekListChild = new List<Week>();

            // Calling the streamreader methods to populate the data lists
            ingredientStreamreader(ingredientList);
            courseStreamreader(courseList);
            courseRestrictionStreamreader(restrictionListBreakfastOne, restrictionListBreakfastTwo, restrictionListLunchOne, restrictionListLunchTwo, restrictionListLunchThree, restrictionListDinnerOne, restrictionListDinnerTwo, restrictionListDinnerThree, restrictionListDinnerFour);

            // Generate a population of chromosomes size N
            int population = 100;
            int generation = 500;
            decimal mutationProb = 0.01M;

            for (int i = 0; i < population; i++)
            {
                createWeek(weekListParent, dayList, courseList, ingredientList, restrictionListBreakfastOne, restrictionListBreakfastTwo, restrictionListLunchOne, restrictionListLunchTwo, restrictionListLunchThree, restrictionListDinnerOne, restrictionListDinnerTwo, restrictionListDinnerThree, restrictionListDinnerFour);
            }

            // Object that holds the lowest priced chromosome for the original population
            int chromosomeNumber = listNumber(weekListParent);
            Week cheapestWeek = new Week { dayOne = weekListParent[chromosomeNumber].dayOne, dayTwo = weekListParent[chromosomeNumber].dayTwo, dayThree = weekListParent[chromosomeNumber].dayThree, dayFour = weekListParent[chromosomeNumber].dayFour, dayFive = weekListParent[chromosomeNumber].dayFive, daySix = weekListParent[chromosomeNumber].daySix, daySeven = weekListParent[chromosomeNumber].daySeven, fitness = weekListParent[chromosomeNumber].fitness, price = weekListParent[chromosomeNumber].price };

            for (int k = 0; k < generation; k++)
            {
                for (int j = 0; j < population; j++)
                {
                    weekListTemp.Clear();

                    rouletteWheel(weekListParent, weekListTemp);
                    crossoverOperator(weekListTemp, weekListChild, mutationProb, ingredientList, courseList, dayList, restrictionListBreakfastOne, restrictionListBreakfastTwo, restrictionListLunchOne, restrictionListLunchTwo, restrictionListLunchThree, restrictionListDinnerOne, restrictionListDinnerTwo, restrictionListDinnerThree, restrictionListDinnerFour);
                    j++;
                }

                weekListParent.Clear();
                weekListParent.AddRange(weekListChild);
                weekListChild.Clear();

                int lowestPrice = listNumber(weekListParent);

                if (weekListParent[lowestPrice].price < cheapestWeek.price)
                {
                    cheapestWeek = weekListParent[lowestPrice];
                }

                int generationCount = k + 1;
                Console.WriteLine("Generation " + generationCount + " = " + weekListParent[lowestPrice].price);
            }

            Console.WriteLine("\nThe cheapest weekly grocery shop");
            Console.WriteLine("Price \t= \t$" + cheapestWeek.price);
            Console.WriteLine("Day One");
            Console.WriteLine("\t" + cheapestWeek.dayOne.courseBreakfastOne);
            Console.WriteLine("\t" + cheapestWeek.dayOne.courseBreakfastTwo);
            Console.WriteLine("\t" + cheapestWeek.dayOne.courseLunchOne);
            Console.WriteLine("\t" + cheapestWeek.dayOne.courseLunchTwo);
            Console.WriteLine("\t" + cheapestWeek.dayOne.courseLunchThree);
            Console.WriteLine("\t" + cheapestWeek.dayOne.courseDinnerOne);
            Console.WriteLine("\t" + cheapestWeek.dayOne.courseDinnerTwo);
            Console.WriteLine("\t" + cheapestWeek.dayOne.courseDinnerThree);
            Console.WriteLine("\t" + cheapestWeek.dayOne.courseDinnerFour);
            Console.WriteLine("Day Two");
            Console.WriteLine("\t" + cheapestWeek.dayTwo.courseBreakfastOne);
            Console.WriteLine("\t" + cheapestWeek.dayTwo.courseBreakfastTwo);
            Console.WriteLine("\t" + cheapestWeek.dayTwo.courseLunchOne);
            Console.WriteLine("\t" + cheapestWeek.dayTwo.courseLunchTwo);
            Console.WriteLine("\t" + cheapestWeek.dayTwo.courseLunchThree);
            Console.WriteLine("\t" + cheapestWeek.dayTwo.courseDinnerOne);
            Console.WriteLine("\t" + cheapestWeek.dayTwo.courseDinnerTwo);
            Console.WriteLine("\t" + cheapestWeek.dayTwo.courseDinnerThree);
            Console.WriteLine("\t" + cheapestWeek.dayTwo.courseDinnerFour);
            Console.WriteLine("Day Three");
            Console.WriteLine("\t" + cheapestWeek.dayThree.courseBreakfastOne);
            Console.WriteLine("\t" + cheapestWeek.dayThree.courseBreakfastTwo);
            Console.WriteLine("\t" + cheapestWeek.dayThree.courseLunchOne);
            Console.WriteLine("\t" + cheapestWeek.dayThree.courseLunchTwo);
            Console.WriteLine("\t" + cheapestWeek.dayThree.courseLunchThree);
            Console.WriteLine("\t" + cheapestWeek.dayThree.courseDinnerOne);
            Console.WriteLine("\t" + cheapestWeek.dayThree.courseDinnerTwo);
            Console.WriteLine("\t" + cheapestWeek.dayThree.courseDinnerThree);
            Console.WriteLine("\t" + cheapestWeek.dayThree.courseDinnerFour);
            Console.WriteLine("Day Four");
            Console.WriteLine("\t" + cheapestWeek.dayFour.courseBreakfastOne);
            Console.WriteLine("\t" + cheapestWeek.dayFour.courseBreakfastTwo);
            Console.WriteLine("\t" + cheapestWeek.dayFour.courseLunchOne);
            Console.WriteLine("\t" + cheapestWeek.dayFour.courseLunchTwo);
            Console.WriteLine("\t" + cheapestWeek.dayFour.courseLunchThree);
            Console.WriteLine("\t" + cheapestWeek.dayFour.courseDinnerOne);
            Console.WriteLine("\t" + cheapestWeek.dayFour.courseDinnerTwo);
            Console.WriteLine("\t" + cheapestWeek.dayFour.courseDinnerThree);
            Console.WriteLine("\t" + cheapestWeek.dayFour.courseDinnerFour);
            Console.WriteLine("Day Five");
            Console.WriteLine("\t" + cheapestWeek.dayFive.courseBreakfastOne);
            Console.WriteLine("\t" + cheapestWeek.dayFive.courseBreakfastTwo);
            Console.WriteLine("\t" + cheapestWeek.dayFive.courseLunchOne);
            Console.WriteLine("\t" + cheapestWeek.dayFive.courseLunchTwo);
            Console.WriteLine("\t" + cheapestWeek.dayFive.courseLunchThree);
            Console.WriteLine("\t" + cheapestWeek.dayFive.courseDinnerOne);
            Console.WriteLine("\t" + cheapestWeek.dayFive.courseDinnerTwo);
            Console.WriteLine("\t" + cheapestWeek.dayFive.courseDinnerThree);
            Console.WriteLine("\t" + cheapestWeek.dayFive.courseDinnerFour);
            Console.WriteLine("Day Six");
            Console.WriteLine("\t" + cheapestWeek.daySix.courseBreakfastOne);
            Console.WriteLine("\t" + cheapestWeek.daySix.courseBreakfastTwo);
            Console.WriteLine("\t" + cheapestWeek.daySix.courseLunchOne);
            Console.WriteLine("\t" + cheapestWeek.daySix.courseLunchTwo);
            Console.WriteLine("\t" + cheapestWeek.daySix.courseLunchThree);
            Console.WriteLine("\t" + cheapestWeek.daySix.courseDinnerOne);
            Console.WriteLine("\t" + cheapestWeek.daySix.courseDinnerTwo);
            Console.WriteLine("\t" + cheapestWeek.daySix.courseDinnerThree);
            Console.WriteLine("\t" + cheapestWeek.daySix.courseDinnerFour);
            Console.WriteLine("Day Seven");
            Console.WriteLine("\t" + cheapestWeek.daySeven.courseBreakfastOne);
            Console.WriteLine("\t" + cheapestWeek.daySeven.courseBreakfastTwo);
            Console.WriteLine("\t" + cheapestWeek.daySeven.courseLunchOne);
            Console.WriteLine("\t" + cheapestWeek.daySeven.courseLunchTwo);
            Console.WriteLine("\t" + cheapestWeek.daySeven.courseLunchThree);
            Console.WriteLine("\t" + cheapestWeek.daySeven.courseDinnerOne);
            Console.WriteLine("\t" + cheapestWeek.daySeven.courseDinnerTwo);
            Console.WriteLine("\t" + cheapestWeek.daySeven.courseDinnerThree);
            Console.WriteLine("\t" + cheapestWeek.daySeven.courseDinnerFour);

            Console.ReadKey();
        }

        static void ingredientStreamreader(List<Ingredient> list)
        {
            // Open the ingredients file into a streamreader
            using (System.IO.StreamReader sr = new System.IO.StreamReader(@"..\..\..\Data_Files\ingredients.txt"))
            {
                while (!sr.EndOfStream) // Keep reading until we get to the end
                {
                    string line = sr.ReadLine();
                    string[] splitLine = line.Split(','); //Split at the commas
                    list.Add(new Ingredient { ingredientName = splitLine[0].Trim(), ingredientPrice = Convert.ToDecimal(splitLine[1].Trim()) });
                }
            }
        }

        static void courseStreamreader(List<Course> list)
        {
            // Open the courses file into a streamreader
            using (System.IO.StreamReader sr = new System.IO.StreamReader(@"..\..\..\Data_Files\courses.txt"))
            {
                while (!sr.EndOfStream) // Keep reading until we get to the end
                {
                    string line = sr.ReadLine();
                    string[] splitLine = line.Split(','); //Split at the commas

                    list.Add(new Course { courseName = splitLine[0].Trim(), courseIngredient = splitLine[1].Trim(), courseUnit = Convert.ToDecimal(splitLine[2].Trim()) });
                }
            }
        }

        static void courseRestrictionStreamreader(List<CourseRestriction> listB1, List<CourseRestriction> listB2, List<CourseRestriction> listL1, List<CourseRestriction> listL2, List<CourseRestriction> listL3, List<CourseRestriction> listD1, List<CourseRestriction> listD2, List<CourseRestriction> listD3, List<CourseRestriction> listD4)
        {
            // Open the restrictions file into a streamreader
            using (System.IO.StreamReader sr = new System.IO.StreamReader(@"..\..\..\Data_Files\courses_restriction.txt"))
            {
                while (!sr.EndOfStream) // Keep reading until we get to the end
                {
                    string line = sr.ReadLine();
                    string[] splitLine = line.Split(','); //Split at the commas

                    if (splitLine[1].Trim() == "breakfast")
                    {
                        if (splitLine[2].Trim() == "1")
                        {
                            listB1.Add(new CourseRestriction { restrictionName = splitLine[0].Trim(), restrictionMeal = splitLine[1].Trim(), restrictionOrder = Convert.ToInt32(splitLine[2].Trim()) });
                        }
                        else if (splitLine[2].Trim() == "2")
                        {
                            listB2.Add(new CourseRestriction { restrictionName = splitLine[0].Trim(), restrictionMeal = splitLine[1].Trim(), restrictionOrder = Convert.ToInt32(splitLine[2].Trim()) });
                        }
                    }
                    else if (splitLine[1].Trim() == "lunch")
                    {
                        if (splitLine[2].Trim() == "1")
                        {
                            listL1.Add(new CourseRestriction { restrictionName = splitLine[0].Trim(), restrictionMeal = splitLine[1].Trim(), restrictionOrder = Convert.ToInt32(splitLine[2].Trim()) });
                        }
                        else if (splitLine[2].Trim() == "2")
                        {
                            listL2.Add(new CourseRestriction { restrictionName = splitLine[0].Trim(), restrictionMeal = splitLine[1].Trim(), restrictionOrder = Convert.ToInt32(splitLine[2].Trim()) });
                        }
                        else if (splitLine[2].Trim() == "3")
                        {
                            listL3.Add(new CourseRestriction { restrictionName = splitLine[0].Trim(), restrictionMeal = splitLine[1].Trim(), restrictionOrder = Convert.ToInt32(splitLine[2].Trim()) });
                        }
                    }
                    else if (splitLine[1].Trim() == "dinner")
                    {
                        if (splitLine[2].Trim() == "1")
                        {
                            listD1.Add(new CourseRestriction { restrictionName = splitLine[0].Trim(), restrictionMeal = splitLine[1].Trim(), restrictionOrder = Convert.ToInt32(splitLine[2].Trim()) });
                        }
                        if (splitLine[2].Trim() == "2")
                        {
                            listD2.Add(new CourseRestriction { restrictionName = splitLine[0].Trim(), restrictionMeal = splitLine[1].Trim(), restrictionOrder = Convert.ToInt32(splitLine[2].Trim()) });
                        }
                        if (splitLine[2].Trim() == "3")
                        {
                            listD3.Add(new CourseRestriction { restrictionName = splitLine[0].Trim(), restrictionMeal = splitLine[1].Trim(), restrictionOrder = Convert.ToInt32(splitLine[2].Trim()) });
                        }
                        if (splitLine[2].Trim() == "4")
                        {
                            listD4.Add(new CourseRestriction { restrictionName = splitLine[0].Trim(), restrictionMeal = splitLine[1].Trim(), restrictionOrder = Convert.ToInt32(splitLine[2].Trim()) });
                        }
                    }
                }
            }
        }

        static string randomCourse(List<CourseRestriction> list)
        {
            // Uses a random number to call a course from the course restriction list
            int random = GetRandomNumber(0, list.Count);
            string courseName = list[random].restrictionName;

            // Returns the random course name
            return courseName;
        }

        static void createDay(List<Day> dayList, List<CourseRestriction> listB1, List<CourseRestriction> listB2, List<CourseRestriction> listL1, List<CourseRestriction> listL2, List<CourseRestriction> listL3, List<CourseRestriction> listD1, List<CourseRestriction> listD2, List<CourseRestriction> listD3, List<CourseRestriction> listD4)
        {
            // Create a temporary list to use to create each course and check whether they are all unique
            // before adding it to the dayList
            List<string> dayListCheck = new List<string>();

            dayListCheck.Add(randomCourse(listB1));
            dayListCheck.Add(randomCourse(listB2));
            dayListCheck.Add(randomCourse(listL1));
            dayListCheck.Add(randomCourse(listL2));
            dayListCheck.Add(randomCourse(listL3));
            dayListCheck.Add(randomCourse(listD1));
            dayListCheck.Add(randomCourse(listD2));
            dayListCheck.Add(randomCourse(listD3));
            dayListCheck.Add(randomCourse(listD4));

            // Calls the checkDay method to make sure each course is unique
            // If not, then this method is recursively called until the checkDay method returns True
            if (checkDay(dayListCheck))
            {
                dayList.Add(new Day { courseBreakfastOne = dayListCheck[0], courseBreakfastTwo = dayListCheck[1], courseLunchOne = dayListCheck[2], courseLunchTwo = dayListCheck[3], courseLunchThree = dayListCheck[4], courseDinnerOne = dayListCheck[5], courseDinnerTwo = dayListCheck[6], courseDinnerThree = dayListCheck[7], courseDinnerFour = dayListCheck[8] });
            }
            else
            {
                createDay(dayList, listB1, listB2, listL1, listL2, listL3, listD1, listD2, listD3, listD4);
            }
        }

        static void createWeek(List<Week> weekList, List<Day> dayList, List<Course> courseList, List<Ingredient> ingredientList, List<CourseRestriction> listB1, List<CourseRestriction> listB2, List<CourseRestriction> listL1, List<CourseRestriction> listL2, List<CourseRestriction> listL3, List<CourseRestriction> listD1, List<CourseRestriction> listD2, List<CourseRestriction> listD3, List<CourseRestriction> listD4)
        {
            dayList.Clear();

            // Loops 7 times to create a week of meals
            for (int i = 0; i < 7; i++)
            {
                createDay(dayList, listB1, listB2, listL1, listL2, listL3, listD1, listD2, listD3, listD4);
            }



            // Calls the checkWeek method to make sure each meal is unique
            // If not, then this method is recursively called until the checkWeek method returns True
            if (checkWeek(dayList))
            {
                decimal price = findPrice(dayList, courseList, ingredientList);
                decimal fitness = fitnessFunction(price);
                weekList.Add(new Week { dayOne = dayList[0], dayTwo = dayList[1], dayThree = dayList[2], dayFour = dayList[3], dayFive = dayList[4], daySix = dayList[5], daySeven = dayList[6], price = price, fitness = fitness });
            }
            else
            {
                createWeek(weekList, dayList, courseList, ingredientList, listB1, listB2, listL1, listL2, listL3, listD1, listD2, listD3, listD4);
            }

        }

        static bool checkDay(List<string> list)
        {
            // Method to find whether each course within the day is unique
            for (int i = 0; i < 9; i++)
            {
                for (int j = i + 1; j < 9; j++)
                {
                    if (list[i] == list[j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static bool checkWeek(List<Day> list)
        {
            // Method to find whether each breakfast meal within the week is unique
            for (int i = 0; i < 7; i++)
            {
                for (int j = i + 1; j < 7; j++)
                {
                    Day dayi = list[i];
                    Day dayj = list[j];

                    if (dayi.courseBreakfastOne == dayj.courseBreakfastOne)
                    {
                        if (dayi.courseBreakfastTwo == dayj.courseBreakfastTwo)
                        {
                            return false;
                        }
                    }
                    else if (dayi.courseLunchOne == dayj.courseLunchOne)
                    {
                        if (dayi.courseLunchTwo == dayj.courseLunchTwo)
                        {
                            if (dayi.courseLunchThree == dayj.courseLunchThree)
                            {
                                return false;
                            }
                        }
                    }
                    else if (dayi.courseDinnerOne == dayj.courseDinnerOne)
                    {
                        if (dayi.courseDinnerTwo == dayj.courseDinnerTwo)
                        {
                            if (dayi.courseDinnerThree == dayj.courseDinnerThree)
                            {
                                if (dayi.courseDinnerFour == dayi.courseDinnerFour)
                                {
                                    return false;
                                }
                            }
                        }
                    }


                }
            }
            return true;
        }

        static decimal findPrice(List<Day> dayList, List<Course> courseList, List<Ingredient> ingredientList)
        {
            List<string> tempCourseIngred = new List<string>();
            List<decimal> tempCourseUnit = new List<decimal>();

            decimal price = 0;

            for (int i = 0; i < 7; i++)
            {
                tempCourseIngred.Clear();
                tempCourseUnit.Clear();
                Day day = dayList[i];

                getIngredients(courseList, tempCourseIngred, tempCourseUnit, day);

                // Loop through the ingredientList to calculate the price of the day
                for (int j = 0; j < tempCourseIngred.Count; j++)
                {
                    for (int k = 0; k < ingredientList.Count; k++)
                    {
                        if (tempCourseIngred[j] == ingredientList[k].ingredientName)
                        {
                            decimal tempPrice = ingredientList[k].ingredientPrice * tempCourseUnit[j];
                            price = price + tempPrice;
                            break;
                        }
                    }
                }
            }


            return price;

        }

        static void getIngredients(List<Course> courseList, List<string> tempCourseIngred, List<decimal> tempCourseUnit, Day day)
        {
            List<string> mealCourseList = new List<string>();
            mealCourseList.Add(day.courseBreakfastOne);
            mealCourseList.Add(day.courseBreakfastTwo);
            mealCourseList.Add(day.courseLunchOne);
            mealCourseList.Add(day.courseLunchTwo);
            mealCourseList.Add(day.courseLunchThree);
            mealCourseList.Add(day.courseDinnerOne);
            mealCourseList.Add(day.courseDinnerTwo);
            mealCourseList.Add(day.courseDinnerThree);
            mealCourseList.Add(day.courseDinnerFour);

            for (int i = 0; i < mealCourseList.Count; i++)
            {
                for (int j = 0; j < courseList.Count; j++)
                {
                    Course course = courseList[j];
                    if (mealCourseList[i] == course.courseName)
                    {
                        while (mealCourseList[i] == course.courseName)
                        {
                            tempCourseIngred.Add(course.courseIngredient);
                            tempCourseUnit.Add(course.courseUnit);
                            j++;
                            if (j >= courseList.Count)
                            {
                                break;
                            }
                            else
                            {
                                course = courseList[j];
                            }
                        }
                    }
                }
            }
        }

        static decimal fitnessFunction(decimal price)
        {
            decimal fitness = 1 / price;

            return fitness;
        }

        static void rouletteWheel(List<Week> weekListParent, List<Week> weekListTemp)
        {
            weekListTemp.Clear();
            List<decimal> fitnessList = new List<decimal>();
            
            for (int i = 0; i < 2; i++)
            {
                fitnessList.Clear();
                int randomTotal = weekListParent.Count * 1000;
                decimal currentFitness = 0;
                decimal totalFitness = 0;

                for (int j = 0; j < weekListParent.Count; j++)
                {
                    totalFitness = totalFitness + weekListParent[j].fitness;
                }

                for (int k = 0; k < weekListParent.Count; k++)
                {
                    decimal fitness = (weekListParent[k].fitness / totalFitness) * 100;
                    fitnessList.Add(fitness);
                }

                decimal random = getrandom.Next(0, randomTotal);
                random = random / 1000;


                for (int l = 0; l < weekListParent.Count; l++)
                {
                    currentFitness += fitnessList[l];
                    if (currentFitness > random)
                    {
                        weekListTemp.Add(weekListParent[l]);
                        weekListParent.RemoveAt(l);
                        break;
                    }
                }
            }
        }

        static void crossoverOperator(List<Week> weekListTemp, List<Week> weekListChild, decimal mutationProb, List<Ingredient> ingredientList, List<Course> courseList, List<Day> dayList, List<CourseRestriction> listB1, List<CourseRestriction> listB2, List<CourseRestriction> listL1, List<CourseRestriction> listL2, List<CourseRestriction> listL3, List<CourseRestriction> listD1, List<CourseRestriction> listD2, List<CourseRestriction> listD3, List<CourseRestriction> listD4)
        {
            // Generates temporary lists
            List<Day> oldWeek1DayList = new List<Day>();
            List<Day> oldWeek2DayList = new List<Day>();
            List<Day> newWeek1DayList = new List<Day>();
            List<Day> newWeek2DayList = new List<Day>();

            // Initialises weeks into variables
            Week oldWeek1 = weekListTemp[0];
            Week oldWeek2 = weekListTemp[1];

            // Populates list with old meals          
            oldWeek1DayList.Add(oldWeek1.dayOne);
            oldWeek1DayList.Add(oldWeek1.dayTwo);
            oldWeek1DayList.Add(oldWeek1.dayThree);
            oldWeek1DayList.Add(oldWeek1.dayFour);
            oldWeek1DayList.Add(oldWeek1.dayFive);
            oldWeek1DayList.Add(oldWeek1.daySix);
            oldWeek1DayList.Add(oldWeek1.daySeven);
            oldWeek2DayList.Add(oldWeek2.dayOne);
            oldWeek2DayList.Add(oldWeek2.dayTwo);
            oldWeek2DayList.Add(oldWeek2.dayThree);
            oldWeek2DayList.Add(oldWeek2.dayFour);
            oldWeek2DayList.Add(oldWeek2.dayFive);
            oldWeek2DayList.Add(oldWeek2.daySix);
            oldWeek2DayList.Add(oldWeek2.daySeven);
            
            // Generates random number to decide where the crossover occurs
            int randomDay = GetRandomNumber(0, 6);
            int randomMeal = GetRandomNumber(0, 2);

            for (int i = 0; i < 7; i++)
            {
                if (i == randomDay)
                {
                    Day oldDay1 = oldWeek1DayList[i];
                    Day oldDay2 = oldWeek2DayList[i];
                    List<string> newDay1List = new List<string>();
                    List<string> newDay2List = new List<string>();

                    if (randomMeal == 0)
                    {
                        // Fill the new day variables
                        newDay1List.Add(oldDay2.courseBreakfastOne);
                        newDay1List.Add(oldDay2.courseBreakfastTwo);
                        newDay1List.Add(oldDay2.courseLunchOne);
                        newDay1List.Add(oldDay2.courseLunchTwo);
                        newDay1List.Add(oldDay2.courseLunchThree);
                        newDay1List.Add(oldDay2.courseDinnerOne);
                        newDay1List.Add(oldDay2.courseDinnerTwo);
                        newDay1List.Add(oldDay2.courseDinnerThree);
                        newDay1List.Add(oldDay2.courseDinnerFour);

                        newDay2List.Add(oldDay1.courseBreakfastOne);
                        newDay2List.Add(oldDay1.courseBreakfastTwo);
                        newDay2List.Add(oldDay1.courseLunchOne);
                        newDay2List.Add(oldDay1.courseLunchTwo);
                        newDay2List.Add(oldDay1.courseLunchThree);
                        newDay2List.Add(oldDay1.courseDinnerOne);
                        newDay2List.Add(oldDay1.courseDinnerTwo);
                        newDay2List.Add(oldDay1.courseDinnerThree);
                        newDay2List.Add(oldDay1.courseDinnerFour);

                        if (checkDay(newDay1List))
                        {
                            if (checkDay(newDay2List))
                            {
                                Day day1 = new Day { courseBreakfastOne = newDay1List[0], courseBreakfastTwo = newDay1List[1], courseLunchOne = newDay1List[2], courseLunchTwo = newDay1List[3], courseLunchThree = newDay1List[4], courseDinnerOne = newDay1List[5], courseDinnerTwo = newDay1List[6], courseDinnerThree = newDay1List[7], courseDinnerFour = newDay1List[8] };
                                Day day2 = new Day { courseBreakfastOne = newDay2List[0], courseBreakfastTwo = newDay2List[1], courseLunchOne = newDay2List[2], courseLunchTwo = newDay2List[3], courseLunchThree = newDay2List[4], courseDinnerOne = newDay2List[5], courseDinnerTwo = newDay2List[6], courseDinnerThree = newDay2List[7], courseDinnerFour = newDay2List[8] };

                                newWeek1DayList.Add(day1);
                                newWeek2DayList.Add(day2);
                            }
                            else
                            {
                                newWeek1DayList.Add(oldDay1);
                                newWeek2DayList.Add(oldDay2);
                            }                                
                        }
                        else
                        {
                            newWeek1DayList.Add(oldDay1);
                            newWeek2DayList.Add(oldDay2);
                        }
                    }
                    else if (randomMeal == 1)
                    {
                        // Fill the new day variables
                        newDay1List.Add(oldDay1.courseBreakfastOne);
                        newDay1List.Add(oldDay1.courseBreakfastTwo);
                        newDay1List.Add(oldDay2.courseLunchOne);
                        newDay1List.Add(oldDay2.courseLunchTwo);
                        newDay1List.Add(oldDay2.courseLunchThree);
                        newDay1List.Add(oldDay2.courseDinnerOne);
                        newDay1List.Add(oldDay2.courseDinnerTwo);
                        newDay1List.Add(oldDay2.courseDinnerThree);
                        newDay1List.Add(oldDay2.courseDinnerFour);

                        newDay2List.Add(oldDay2.courseBreakfastOne);
                        newDay2List.Add(oldDay2.courseBreakfastTwo);
                        newDay2List.Add(oldDay1.courseLunchOne);
                        newDay2List.Add(oldDay1.courseLunchTwo);
                        newDay2List.Add(oldDay1.courseLunchThree);
                        newDay2List.Add(oldDay1.courseDinnerOne);
                        newDay2List.Add(oldDay1.courseDinnerTwo);
                        newDay2List.Add(oldDay1.courseDinnerThree);
                        newDay2List.Add(oldDay1.courseDinnerFour);

                        if (checkDay(newDay1List))
                        {
                            if (checkDay(newDay2List))
                            {
                                Day day1 = new Day { courseBreakfastOne = newDay1List[0], courseBreakfastTwo = newDay1List[1], courseLunchOne = newDay1List[2], courseLunchTwo = newDay1List[3], courseLunchThree = newDay1List[4], courseDinnerOne = newDay1List[5], courseDinnerTwo = newDay1List[6], courseDinnerThree = newDay1List[7], courseDinnerFour = newDay1List[8] };
                                Day day2 = new Day { courseBreakfastOne = newDay2List[0], courseBreakfastTwo = newDay2List[1], courseLunchOne = newDay2List[2], courseLunchTwo = newDay2List[3], courseLunchThree = newDay2List[4], courseDinnerOne = newDay2List[5], courseDinnerTwo = newDay2List[6], courseDinnerThree = newDay2List[7], courseDinnerFour = newDay2List[8] };

                                newWeek1DayList.Add(day1);
                                newWeek2DayList.Add(day2);
                            }
                            else
                            {
                                newWeek1DayList.Add(oldDay1);
                                newWeek2DayList.Add(oldDay2);
                            }
                        }
                        else
                        {
                            newWeek1DayList.Add(oldDay1);
                            newWeek2DayList.Add(oldDay2);
                        }
                    }
                    else
                    {
                        // Fill the new day variables
                        newDay1List.Add(oldDay1.courseBreakfastOne);
                        newDay1List.Add(oldDay1.courseBreakfastTwo);
                        newDay1List.Add(oldDay1.courseLunchOne);
                        newDay1List.Add(oldDay1.courseLunchTwo);
                        newDay1List.Add(oldDay1.courseLunchThree);
                        newDay1List.Add(oldDay2.courseDinnerOne);
                        newDay1List.Add(oldDay2.courseDinnerTwo);
                        newDay1List.Add(oldDay2.courseDinnerThree);
                        newDay1List.Add(oldDay2.courseDinnerFour);

                        newDay2List.Add(oldDay2.courseBreakfastOne);
                        newDay2List.Add(oldDay2.courseBreakfastTwo);
                        newDay2List.Add(oldDay2.courseLunchOne);
                        newDay2List.Add(oldDay2.courseLunchTwo);
                        newDay2List.Add(oldDay2.courseLunchThree);
                        newDay2List.Add(oldDay1.courseDinnerOne);
                        newDay2List.Add(oldDay1.courseDinnerTwo);
                        newDay2List.Add(oldDay1.courseDinnerThree);
                        newDay2List.Add(oldDay1.courseDinnerFour);

                        if (checkDay(newDay1List))
                        {
                            if (checkDay(newDay2List))
                            {
                                Day day1 = new Day { courseBreakfastOne = newDay1List[0], courseBreakfastTwo = newDay1List[1], courseLunchOne = newDay1List[2], courseLunchTwo = newDay1List[3], courseLunchThree = newDay1List[4], courseDinnerOne = newDay1List[5], courseDinnerTwo = newDay1List[6], courseDinnerThree = newDay1List[7], courseDinnerFour = newDay1List[8] };
                                Day day2 = new Day { courseBreakfastOne = newDay2List[0], courseBreakfastTwo = newDay2List[1], courseLunchOne = newDay2List[2], courseLunchTwo = newDay2List[3], courseLunchThree = newDay2List[4], courseDinnerOne = newDay2List[5], courseDinnerTwo = newDay2List[6], courseDinnerThree = newDay2List[7], courseDinnerFour = newDay2List[8] };

                                newWeek1DayList.Add(day1);
                                newWeek2DayList.Add(day2);
                            }
                            else
                            {
                                newWeek1DayList.Add(oldDay1);
                                newWeek2DayList.Add(oldDay2);
                            }
                        }
                        else
                        {
                            newWeek1DayList.Add(oldDay1);
                            newWeek2DayList.Add(oldDay2);
                        }
                    }
                }
                else
                {
                    newWeek1DayList.Add(oldWeek1DayList[i]);
                    newWeek2DayList.Add(oldWeek2DayList[i]);
                }
            }

            weekListTemp.Add(new Week { dayOne = newWeek1DayList[0], dayTwo = newWeek1DayList[1], dayThree = newWeek1DayList[2], dayFour = newWeek1DayList[3], dayFive = newWeek1DayList[4], daySix = newWeek1DayList[5], daySeven = newWeek1DayList[6] });
            weekListTemp.Add(new Week { dayOne = newWeek2DayList[0], dayTwo = newWeek2DayList[1], dayThree = newWeek2DayList[2], dayFour = newWeek2DayList[3], dayFive = newWeek2DayList[4], daySix = newWeek2DayList[5], daySeven = newWeek2DayList[6] });

            mutationCrossover(weekListTemp[2], weekListChild, ingredientList, courseList, mutationProb, listB1, listB2, listL1, listL2, listL3, listD1, listD2, listD3, listD4);
            mutationCrossover(weekListTemp[3], weekListChild, ingredientList, courseList, mutationProb, listB1, listB2, listL1, listL2, listL3, listD1, listD2, listD3, listD4);
        }

        static void mutationCrossover(Week weekTemp, List<Week> weekListChild, List<Ingredient> ingredientList, List<Course> courseList, decimal mutationProb, List<CourseRestriction> listB1, List<CourseRestriction> listB2, List<CourseRestriction> listL1, List<CourseRestriction> listL2, List<CourseRestriction> listL3, List<CourseRestriction> listD1, List<CourseRestriction> listD2, List<CourseRestriction> listD3, List<CourseRestriction> listD4)
        {
            // Finds probability of crossover occuring
            decimal randomMutation = GetRandomNumber(0, 1000);
            randomMutation = randomMutation / 1000;

            List<string> newDayListCheck = new List<string>();
            List<Day> newDayList = new List<Day>();
            newDayList.Add(weekTemp.dayOne);
            newDayList.Add(weekTemp.dayTwo);
            newDayList.Add(weekTemp.dayThree);
            newDayList.Add(weekTemp.dayFour);
            newDayList.Add(weekTemp.dayFive);
            newDayList.Add(weekTemp.daySix);
            newDayList.Add(weekTemp.daySeven);

            if (mutationProb >= randomMutation)
            {
                int randomDay = GetRandomNumber(0, 6);
                int randomMeal = GetRandomNumber(0, 2);

                for (int i = 0; i < 7; i++)
                {
                    if (i == randomDay)
                    {
                        newDayListCheck.Clear();

                        if (randomMeal == 0)
                        {
                            newDayListCheck.Add(randomCourse(listB1));
                            newDayListCheck.Add(randomCourse(listB2));
                            newDayListCheck.Add(newDayList[i].courseLunchOne);
                            newDayListCheck.Add(newDayList[i].courseLunchTwo);
                            newDayListCheck.Add(newDayList[i].courseLunchThree);
                            newDayListCheck.Add(newDayList[i].courseDinnerOne);
                            newDayListCheck.Add(newDayList[i].courseDinnerTwo);
                            newDayListCheck.Add(newDayList[i].courseDinnerThree);
                            newDayListCheck.Add(newDayList[i].courseDinnerFour);

                            if (checkDay(newDayListCheck))
                            {
                                newDayList[i] = new Day { courseBreakfastOne = newDayListCheck[0], courseBreakfastTwo = newDayListCheck[1], courseLunchOne = newDayListCheck[2], courseLunchTwo = newDayListCheck[3], courseLunchThree = newDayListCheck[4], courseDinnerOne = newDayListCheck[5], courseDinnerTwo = newDayListCheck[6], courseDinnerThree = newDayListCheck[7], courseDinnerFour = newDayListCheck[8] };
                            }
                            /*else
                            {
                                i = i - 1;
                            }*/
                        }
                        else if (randomMeal == 1)
                        {
                            newDayListCheck.Add(newDayList[i].courseBreakfastOne);
                            newDayListCheck.Add(newDayList[i].courseBreakfastOne);
                            newDayListCheck.Add(randomCourse(listL1));
                            newDayListCheck.Add(randomCourse(listL2));
                            newDayListCheck.Add(randomCourse(listL3));
                            newDayListCheck.Add(newDayList[i].courseDinnerOne);
                            newDayListCheck.Add(newDayList[i].courseDinnerTwo);
                            newDayListCheck.Add(newDayList[i].courseDinnerThree);
                            newDayListCheck.Add(newDayList[i].courseDinnerFour);

                            if (checkDay(newDayListCheck))
                            {
                                newDayList[i] = new Day { courseBreakfastOne = newDayListCheck[0], courseBreakfastTwo = newDayListCheck[1], courseLunchOne = newDayListCheck[2], courseLunchTwo = newDayListCheck[3], courseLunchThree = newDayListCheck[4], courseDinnerOne = newDayListCheck[5], courseDinnerTwo = newDayListCheck[6], courseDinnerThree = newDayListCheck[7], courseDinnerFour = newDayListCheck[8] };
                            }
                            /*else
                            {
                                i = i - 1;
                            }*/
                        }
                        else
                        {
                            newDayListCheck.Add(newDayList[i].courseBreakfastOne);
                            newDayListCheck.Add(newDayList[i].courseBreakfastTwo);
                            newDayListCheck.Add(newDayList[i].courseLunchOne);
                            newDayListCheck.Add(newDayList[i].courseLunchTwo);
                            newDayListCheck.Add(newDayList[i].courseLunchThree);
                            newDayListCheck.Add(randomCourse(listD1));
                            newDayListCheck.Add(randomCourse(listD2));
                            newDayListCheck.Add(randomCourse(listD3));
                            newDayListCheck.Add(randomCourse(listD4));

                            if (checkDay(newDayListCheck))
                            {
                                newDayList[i] = new Day { courseBreakfastOne = newDayListCheck[0], courseBreakfastTwo = newDayListCheck[1], courseLunchOne = newDayListCheck[2], courseLunchTwo = newDayListCheck[3], courseLunchThree = newDayListCheck[4], courseDinnerOne = newDayListCheck[5], courseDinnerTwo = newDayListCheck[6], courseDinnerThree = newDayListCheck[7], courseDinnerFour = newDayListCheck[8] };
                            }
                            /*else
                            {
                                i = i - 1;
                            }*/
                        }
                    }
                }

                if (checkWeek(newDayList))
                {
                    decimal price = findPrice(newDayList, courseList, ingredientList);
                    decimal fitness = fitnessFunction(price);
                    weekListChild.Add(new Week { dayOne = newDayList[0], dayTwo = newDayList[1], dayThree = newDayList[2], dayFour =newDayList[3], dayFive = newDayList[4], daySix = newDayList[5], daySeven = newDayList[6], price = price, fitness = fitness });
                }
                else
                {
                    mutationCrossover(weekTemp, weekListChild, ingredientList, courseList, mutationProb, listB1, listB2, listL1, listL2, listL3, listD1, listD2, listD3, listD4);
                }
            }
            else
            {
                decimal price = findPrice(newDayList, courseList, ingredientList);
                decimal fitness = fitnessFunction(price);
                weekListChild.Add(new Week { dayOne = newDayList[0], dayTwo = newDayList[1], dayThree = newDayList[2], dayFour = newDayList[3], dayFive = newDayList[4], daySix = newDayList[5], daySeven = newDayList[6], price = price, fitness = fitness });
            }
        }

        static int listNumber(List<Week> list)
        {
            decimal price1 = list[0].price;
            int listNumber = 0;

            for (int i = 1; i < list.Count; i++)
            {
                decimal price2 = list[i].price;
                
                if (price1 > price2)
                {
                    price1 = price2;
                    listNumber = i;
                } 
            }

            return listNumber;
        }
    }
}
