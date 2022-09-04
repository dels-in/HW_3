public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Calculate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"В калькуляторе произошла ошибка: {ex.Message}");
        }
    }

    static void Calculate()
    {
        int result = 0;
        string? firstOperand = null;
        string? operators = null;
        string? secondOperand=null;
        
        Console.WriteLine("Для выхода из калькулятора введите \"стоп\"");
        
        void Sum(int a, int b)
        {
            try
            {
                checked
                {
                    result = a + b;
                }
            }
            catch (OverflowException ex)
            {
                throw ex;
            }
        }

        void Sub(int a, int b)
        {
            try
            {
                checked
                {
                    result = a - b;
                }
            }
            catch (OverflowException ex)
            {
                throw ex;
            }
        }

        void Mul(int a, int b)
        {
            try
            {
                checked
                {
                    result = a * b;
                }
            }
            catch (OverflowException ex)
            {
                throw ex;
            }
        }

        void Div(int a, int b)
        {
            if (b == 0)
            {
                var ex = new DivideByZeroException("Деление на ноль");
                throw ex;
            }
            try
            {
                checked
                {
                    result = a / b;
                }
            }
            catch (OverflowException ex)
            {
                throw ex;
            }
        }

        try
        {
            do
            {
                Console.WriteLine("Введите требуемое выражение в виде \"Операнд Оператор Операнд\"");
                var input = Console.ReadLine();
                string[] keyChar = input.Split(" ");
                firstOperand = keyChar[0];
                operators = keyChar[1];
                secondOperand = keyChar[2];

                //попытался проверить сразу все элементы массива через цикл,
                //но он не возвращает значение true для res за пределами функции, как я понял. очень жаль :(
                //bool res
                // for (int i = 0; i < 2; i++)
                // {
                //     if (keyChar[i] == "стоп")
                //     {
                //         res=true;
                //         break;
                //     }
                // }

                if (firstOperand == "стоп" || operators == "стоп" || secondOperand == "стоп")
                    break;

                //только так смог проверить отсутствие оператора,
                //но тогда все равно требуется нажать пробел после второго введенного числа
                var parsingOperator = int.TryParse(operators, out _);
                if (parsingOperator)
                {
                    secondOperand = "1";
                    var ex = new NullOperatorException("");
                    ex.Data.Add("data", "Укажите в выражении оператор: +, -, *, /");
                    throw ex;
                }

                //не получилось по-другому проверить, число ли операнд :(
                var formatA = long.TryParse(firstOperand, out _);
                var formatB = long.TryParse(secondOperand, out _);

                if (formatA != true)
                {
                    var ex = new FormatOperandException("");
                    ex.Data.Add("data", $"Операнд {firstOperand} не является числом");
                    throw ex;
                }

                if (formatB != true)
                {
                    var ex = new FormatOperandException("");
                    ex.Data.Add("data", $"Операнд {secondOperand} не является числом");
                    throw ex;
                }

                var a = unchecked((int)long.Parse(firstOperand));
                var b = unchecked((int)long.Parse(secondOperand));

                if (operators != "+" && operators != "-" && operators != "*" && operators != "/")
                {
                    var ex = new InvalidOperatorException($"Я пока не умею работать с оператором {operators}");
                    throw ex;
                }

                if (operators == "+")
                    Sum(a, b);
                else if (operators == "-")
                    Sub(a, b);
                else if (operators == "*")
                    Mul(a, b);
                else if (operators == "/")
                    Div(a, b);

                // if (result.GetType() != typeof(int))
                // {
                //     var ex = new OutOfInt("Результат выражения вышел за границы int");
                //     throw ex;
                // }

                Console.WriteLine($"Ответ: {result}");

                if (result == 13)
                {
                    var ex = new Answer13Exception("Вы получили ответ 13!");
                    throw ex;
                }
            } while (firstOperand != "стоп" || operators != "стоп" || secondOperand != "стоп");
        }
        catch (NullOperatorException ex)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(ex.Data["data"]);
            Console.ResetColor();
        }
        catch (InvalidOperatorException ex)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }
        catch (IndexOutOfRangeException)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Выражение некорректное, попробуйте написать в формате: " +
                              "\na + b; \na * b;\na - b;\na / b");
            Console.ResetColor();
        }
        catch (FormatException)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Выражение некорректное, попробуйте написать в формате: " +
                              "\na + b; \na * b;\na - b;\na / b");
            Console.ResetColor();
        }
        catch (FormatOperandException ex)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(ex.Data["data"]);
            Console.ResetColor();
        }
        catch (DivideByZeroException ex)
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }
        catch (Answer13Exception ex)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }
        catch (OverflowException)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Результат выражения вышел за границы int");
            Console.ResetColor();
        }
        catch (Exception)
        {
            var ex = new Exception("Я не смог обработать ошибку");
            throw;
        }
    }
}

class NullOperatorException : Exception
{
    public NullOperatorException(string message) 
        : base(message) {}
}

class InvalidOperatorException : Exception
{
    public InvalidOperatorException(string message) 
        : base(message) {}
}

class FormatOperandException : Exception
{ 
    public FormatOperandException(string message)
        : base(message) {} 
}
class Answer13Exception : Exception
{
    public Answer13Exception(string message) 
        : base(message) {}
}
class OutOfInt : Exception
{
    public OutOfInt(string message) 
        : base(message) {}
}