using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOChain;

namespace Examples_HackerRank.DataStructures.Stack
{
    class CECacheMaximums : CExercise
    {
        public CECacheMaximums () { ID = "Data Structures.Stack.Cache Maximums"; }

        //    Входные данные:
        //-------------------------------------------------------------------------------
        //    N -- длина списка входных команд, 1 <= N <= 10^5
        //    x -- число в команде, 1 <= x <= 10^9
        //    command_type -- тип входящей команды:
        //       1 x  -- вставить элемент x в стек
        //       2    -- удалить верхний элемент из стека
        //       3    -- вывести максимальный элемент, который сейчас находится в стеке


        //    Цель задачи:
        //-------------------------------------------------------------------------------
        //    Найти способ быстро кешировать максимальные значения в стеке.


        //    Смысловое определение и решение:
        //-------------------------------------------------------------------------------
        //    В абстрактный подземный отель приезжают гости с определенным весом.
        //    
        //    При въезде гостя, отель опускается под землю на один этаж,
        //    и новый гость занимает верхний доступный для входа номер.
        //    
        //    Съехать из отеля может только верхний гость. При этом отель поднимается
        //    на один этаж выше.
        //    
        //    В любой момент может приехать "Полиция Жирности", и спросить у хозяина
        //    отеля какой максимальный по весу гость у него проживает. Хозяин обязан
        //    дать ответ сразу, без обхода всех проживающих внутри гостей. 
        //    
        //    Решение: Чтобы иметь возможность сразу отвечать на заданный вопрос
        //    о максимально весомом госте, хозяин должен поставить на вход дворецкого,
        //    который будет записывать в стопку листов особые данные о максимальном
        //    весе. С помощью просмотра верхней записи из этой стопки, можно будет 
        //    дать правильный ответ без обхода гостей внутри отеля.
        //      
        //    Чтобы стопка не была слишком большой, на каждом листе также дублируется
        //    число повторов, если заехало сразу несколько одинаковых гостей. 


        //    Иллюстрация:
        //-------------------------------------------------------------------------------
        //                 pop
        //    Отель:        /\                 Контрольные записи дворецкого:
        //               _________    
        //               | пусто |                 [  __ кг (_ чел)  ]
        //               | пусто |                 [  __ кг (_ чел)  ]
        //               | пусто |                 
        //      въезд    | пусто |    выезд        
        //   xN --> ____]|    10 |[____ -->        [  10 кг (2 чел)  ]  <-- верхний
        //              ||     6 ||                [   2 кг (1 чел)  ]      бланк
        //              ||    10 ||                 
        //              ||     2 ||            Ответ на текущую ситуацию (peek):
        //              |'-------'|                    
        //              |.  \/   .|                В отеле проживает два человека
        //              |. push  .|                с максимальным весом 10 кг. 
        //              |.       .|


        //   Tuple класс до C# 7.0 (Framework 4.7) снижает читаемость кода,
        //   поэтому применяю структуру. 
        private struct Record
        {
            public int Weight;
            public int Count;

            public static Record Empty { get { return new Record { Weight = 1, Count = 0 }; } }
        }



        public override void Execute (CIO io)
        {
            const int max_n = 1_00000;          //  10^5
            const int max_x = 1_000_000_000;    //  10^9

            var empty_record = Record.Empty;


            //  Получаем значение N (кол-во последующих команд) и проверяем его. 
            if (io.NotIn(io.Number(out int n_count), io.InEOL()))
            {
                throw new ArgumentException("N некорректно");
            }

            if ((n_count < 1) || (n_count > max_n))
            {
                throw new ArgumentOutOfRangeException("N вышло за пределы");
            }


            var numbers = new Stack<int> ();
            var max_cache = new Stack<Record> ();


            //  Обработка списка входящих команд
            for (int i = 0; i < n_count; i++)
            {
                if (io.NotIn(io.Number(out int command_type)))
                {
                    throw new InvalidOperationException("Нет входящей команды или она ошибочна");
                }

                switch (command_type)
                {
                    //  Команда добавления числа в стек. 
                    //  "Въезд нового гостя в отель" 
                    case 1:
                    {
                        if (io.NotIn(io.OneOf(" "), io.Number(out int x)))
                        {
                            throw new InvalidOperationException("Аргумент команды #1 ошибочен или отсутствует");
                        }

                        if ((x < 1) || (x > max_x))
                        {
                            throw new InvalidOperationException("Аргумент команды #1 вне диапазона");
                        }

                        //  Гость заселился. 
                        numbers.Push(x);

                        //  Дворецкий берет верхний листок из своих записей
                        Record upper_record = (max_cache.Count > 0) ? max_cache.Peek() : empty_record;

                        //  Если въехавший гость тяжелее по весу, чем текущий максимум,
                        //  создаем новый листок о новом максимуме. 
                        if (x > upper_record.Weight)
                        {
                            max_cache.Push(new Record { Weight = x, Count = 1 });
                        }
                        //  Иначе, если въехавший гость равен по максимуму текущему
                        else if (x == upper_record.Weight)
                        {
                            //  Обновляем или добавляем новый верхний листок в записях дворецкого.
                            //  Счетчик тяжелых гостей увеличен.
                            if (max_cache.Count > 0) max_cache.Pop();
                            upper_record.Count++;
                            max_cache.Push(upper_record);
                        }
                    }
                    break;


                    //  Команда удаления верхнего числа из стека.
                    //  "Верхний гость уезжает из отеля"
                    case 2:
                    {
                        if (numbers.Count <= 0)
                        {
                            throw new InvalidOperationException("Входящая команда на удаление при пустом списке");
                        }

                        //  Гость уезжает. 
                        int x = numbers.Pop();

                        //  Дворецкий исправляет или удаляет верхнюю запись, если это необходимо. 
                        Record upper_record = (max_cache.Count > 0) ? max_cache.Peek() : empty_record;

                        if (x == upper_record.Weight)
                        {
                            //  Обновляем, добавляем новый или убираем совсем верхний листок 
                            //  в записях дворецкого. Счетчик тяжелых гостей уменьшается.
                            if (max_cache.Count > 0) max_cache.Pop();
                            upper_record.Count--;
                            if (upper_record.Count > 0) max_cache.Push(upper_record);
                        }
                    }
                    break;


                    //  Команда запроса значения максимума, находящегося в стеке.
                    //  "Приехала Полиция Жирности, нужно сразу дать ответ, кто самый тяжелый в отеле"
                    case 3:
                    {
                        io.Out
                        (
                            io.Number((max_cache.Count > 0) ? max_cache.Peek().Weight : empty_record.Weight),
                            io.OutEOL()
                        );
                    }
                    break;


                    default:
                    {
                        throw new InvalidOperationException("Ошибочная команда");
                    }
                }

                //  Завершаем чтение команды (текущей строки). 
                if (io.NotIn(io.InEOL()))
                {
                }
            }
        }



        //public interface IInputQuery
        //{
        //    void In ();
        //}

        //public interface IOutputQuery
        //{
        //    void Out ();
        //}

        //public interface IIONumberQuery : IInputQuery, IOutputQuery
        //{
        //    int Number { get; set; }
        //}


        //public interface IInputOutput
        //{
        //    bool In (params IInputQuery[] input_chain);
        //    bool Out (params IOutputQuery[] output_chain);

        //    IIONumberQuery Number (int result_number);


        //    //  Input
        //    bool ReadNumber (out int result_number);
        //    bool ReadLine ();

        //    //  Output
        //    IInputOutput WriteNumber (int number);
        //    IInputOutput WriteLine ();
        //}



        //public class CConsoleInputOutput : IInputOutput
        //{
        //    const char CR = '\r';
        //    const char LF = '\n';
        //    const int CR_Code = (int) CR;
        //    const int LF_Code = (int) LF;

        //    public bool In (params bool[] input_query)
        //    {
        //        foreach (bool i in input_query)
        //        {
        //            if (!i) return false;
        //        }

        //        return true;
        //    }

        //    public bool ReadNumber (out int result_number)
        //    {
        //        var text = new StringBuilder ();

        //        int read;
        //        while ((read = Console.Read()) >= 0)
        //        {
        //            char c = Convert.ToChar(read);
        //            if (char.IsNumber(c) || c == '.')
        //                text.Append(c);
        //            else
        //                break;
        //        }

        //        return int.TryParse(text.ToString(), out result_number);                    
        //    }

        //    public bool ReadLine ()
        //    {
        //        Console.In.ReadLine();
        //        return true;

        //        //int read;
        //        //while ((read = Console.Read()) != LF_Code)
        //        //{
        //        //    if (read < 0) return true;
        //        //}
        //    }

        //    public IInputOutput WriteLine ()
        //    {
        //        Console.Out.Write(Environment.NewLine);
        //        return this;
        //    }

        //    public IInputOutput WriteNumber (int number)
        //    {
        //        Console.Out.Write(number);
        //        return this;
        //    }
        //}
    }
}
