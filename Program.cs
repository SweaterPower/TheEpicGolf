using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace TheEpicGolf
{
    class Program
    {

        public static int score = 0;
        //очки аркады
        public static int streak = 0;
        //попадания подряд
        public static int lifes = 0;
        //жизьки
        public static string profile = "";
        //имя профиля
        public static string currentsave = "";
        //имя загруженного сохранения
        public static int turn = 0;
        //текущий ход
        public static bool sound = false;
        //включение/отключение звука

        static void StartScreen()
        //заставка для красоты
        {
            DirectoryInfo trailer = new DirectoryInfo(@"start_screen\");
            FileInfo[] inputpic = trailer.GetFiles(@"*.txt");
            string st = File.ReadAllText(inputpic[0].FullName);
            Console.WriteLine(st);
            System.Threading.Thread.Sleep(1000);
            Console.Clear();
        }

        static void TypeTitle()
        //печатает заголовок
        {
            Console.WriteLine("╔══════════════════════════════╦════════════════╦═════════════════════════════╗");
            Console.WriteLine("║     Автор: Андрей Дубаков    ║The Epic Golf!!!║        Группа: 144-2        ║");
            Console.WriteLine("╚══════════════════════════════╩════════════════╩═════════════════════════════╝");
        }

        static void WelcomeSign()
        //добро пожаловать
        {
            Console.WriteLine("╔══════════════════════════════╦════════════════╦═════════════════════════════╗");
            Console.WriteLine("║       Добро пожаловать!      ║                ║       Приятной игры!        ║");
            Console.WriteLine("╚══════════════════════════════╩════════════════╩═════════════════════════════╝");
        }

        static void SoundControl()
        //включает или отключает звук
        {
            Console.WriteLine(" * Включить звук?");
            Console.WriteLine(" 1) Да, включить.");
            Console.WriteLine(" 2) Нет, не включать.");
            Console.Write(" * Введите соответствующую цифру.\n > ");
            int control;
            ControlInput(out control);
            if (control == 1) sound = true;
            else sound = false;
            Console.Clear();
            TypeTitle();
        }

        static void MainSound()
        //музыка
        {
            if (sound)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"soundtracks\mainsound_1.wav");
                player.Play();
            }
        }

        static void GoSound()
        //звук начала игры
        {
            if (sound)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"soundtracks\letsgo.wav");
                player.Stop();
                player.Play();
            }
        }

        static void RampageSound()
        //звук при 5 попаданиях подряд
        {
            if (sound)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"soundtracks\rampage.wav");
                player.Stop();
                player.Play();
            }
        }

        static void VictorySound()
        //звук при попадании
        {
            if (sound)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                player.Stop();
                Random rnd = new Random();
                int control = rnd.Next(0, 7);
                switch (control)
                {
                    case 0:
                        player = new System.Media.SoundPlayer(@"soundtracks\ba_seethat.wav");
                        break;
                    case 1:
                        player = new System.Media.SoundPlayer(@"soundtracks\good_shot2.wav");
                        break;
                    case 2:
                        player = new System.Media.SoundPlayer(@"soundtracks\great.wav");
                        break;
                    case 3:
                        player = new System.Media.SoundPlayer(@"soundtracks\well_done.wav");
                        break;
                    case 4:
                        player = new System.Media.SoundPlayer(@"soundtracks\yea_baby.wav");
                        break;
                    case 5:
                        player = new System.Media.SoundPlayer(@"soundtracks\yesss.wav");
                        break;
                    case 6:
                        player = new System.Media.SoundPlayer(@"soundtracks\nice_shot.wav");
                        break;
                    default: ArcadeMenu(); break;
                }
                player.Play();
            }
        }

        static void DefeatSound()
        //звук при промахе
        {
            if (sound)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                player.Stop();
                Random rnd = new Random();
                int control = rnd.Next(0, 6);
                switch (control)
                {
                    case 0:
                        player = new System.Media.SoundPlayer(@"soundtracks\ahh_negative.wav");
                        break;
                    case 1:
                        player = new System.Media.SoundPlayer(@"soundtracks\aw_hell.wav");
                        break;
                    case 2:
                        player = new System.Media.SoundPlayer(@"soundtracks\no.wav");
                        break;
                    case 3:
                        player = new System.Media.SoundPlayer(@"soundtracks\noo.wav");
                        break;
                    case 4:
                        player = new System.Media.SoundPlayer(@"soundtracks\nothing.wav");
                        break;
                    case 5:
                        player = new System.Media.SoundPlayer(@"soundtracks\terwin.wav");
                        break;
                    default: ArcadeMenu(); break;
                }
                player.Play();
            }
        }

        static void GameQuit()
        //выход из игры, так как окно консоли закрывать крайне нежелательно
        {
            int sure = 0;
            Console.WriteLine(" * Вы точно хотите выйти? >> | 1 - Да | 2 - Нет |");
            Console.Write(" > ");
            ControlInput(out sure);
            if (sure == 1)
            {
                Console.WriteLine(" * Игра закрывается...");
                RefreshHighscoreTable();
                Environment.Exit(0);
            }
            else
                if (sure == 2)
                {

                    Console.WriteLine(" * Возврат в меню.");
                    Console.Clear();
                    TypeTitle();
                    if (profile == "") ProfileMenu();
                    else MainMenu();
                }
                else
                {
                    Console.WriteLine(" * Некорректный ввод, возврат в главное меню...");
                    Console.Clear();
                    TypeTitle();
                    if (profile == "") ProfileMenu();
                    else MainMenu();
                }
        }

        static void ControlInput(out int a)
        //профилактика на случай ввода символов во время переходов по меню
        {
            string st = "", res = "";
            st = Console.ReadLine();
            if (st == "menu") MainMenu();
            else
            {
                for (int i = 0; i < st.Length; i++)
                    if (Char.IsDigit(st[i]))
                        res += st[i];
                if (res == "" || res.Length >= 9) res = "0";
            }
            a = Convert.ToInt32(res);
            if (sound) Console.Beep(450, 50);
        }

        static void DigitInput(out double a)
        //защита от некорректного ввода вещественого числа во время игры
        {
            string st = "", res = "";
            st = Console.ReadLine();
            if (st == "menu") MainMenu();
            else
            {
                for (int i = 0; i < st.Length; i++)
                {
                    if (Char.IsDigit(st[i]))
                        res += st[i];
                    else if (st[i] == ',' && st.IndexOf(st[i]) == st.LastIndexOf(st[i]))
                        res += st[i];
                    else if (st[i] == '.' && st.IndexOf(st[i]) == st.LastIndexOf(st[i]))
                        res += ',';
                }
                if (res == "" || res.Length >= 9) res = "0";
            }
            a = Convert.ToDouble(res);
            if (sound) Console.Beep(450, 50);
        }

        static void MainMenu()
        //главное меню
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            Console.WriteLine(" * Выберите действие.");
            int control = 0;
            Console.WriteLine(" 1) Начать игру.\n 2) Таблица рекордов.\n 3) Управление профилями.\n 4) Помощь.\n 5) Выйти из игры.");
            Console.Write(" * Введите соответствующую цифру.\n > ");
            ControlInput(out control);
            switch (control)
            {
                case 1: player.Stop(); GoSound(); StartGame(); break;
                case 2: OutputHighscoreTable(); break;
                case 3: ProfileMenu(); break;
                case 4: HelpMenu(); break;
                case 5: player.Stop(); GameQuit(); break;
                default: Console.Clear();
                    TypeTitle();
                    Console.WriteLine(" * Некорректный ввод.");
                    MainMenu(); break;
            }
        }

        static void RefreshHighscoreTable()
        //обновляет содержимое таблицы рекордов
        {
            DirectoryInfo ProfDir = new DirectoryInfo(@"profiles\");
            DirectoryInfo[] Profiles = ProfDir.GetDirectories();
            int[] highscores = new int[Profiles.Length];
            string[] names = new string[Profiles.Length];
            for (int i = 0; i < Profiles.Length; i++)
            {
                FileInfo CheckRecord = new FileInfo(Profiles[i].FullName + "\\" + Profiles[i].Name + "_arcade.txt");
                StreamReader GetScore = CheckRecord.OpenText();
                highscores[i] = Convert.ToInt32(GetScore.ReadLine());
                names[i] = Profiles[i].Name;
                GetScore.Close();
            }
            int x;
            string st;
            for (int i = 0; i < Profiles.Length; i++)
            {
                for (int j = Profiles.Length - 1; j > i; j--)
                {
                    if (highscores[j - 1] > highscores[j])
                    {
                        x = highscores[j - 1];
                        highscores[j - 1] = highscores[j];
                        highscores[j] = x;
                        st = names[j - 1];
                        names[j - 1] = names[j];
                        names[j] = st;
                    }
                }
            } FileInfo GetTable = new FileInfo(@"highscores\higscore_table.txt");
            GetTable.Delete();
            StreamWriter AddScore = GetTable.AppendText();
            for (int i = highscores.Length - 1; i >= 0; i--)
            {
                AddScore.WriteLine(names[i]);
                AddScore.WriteLine(highscores[i]);
            }
            AddScore.Close();
        }

        static void OutputHighscoreTable()
        //выводит на экран таблицу рекордов
        {
            Console.Clear();
            TypeTitle();
            RefreshHighscoreTable();
            StreamReader GetTable = new StreamReader(@"highscores\higscore_table.txt");
            DirectoryInfo ProfDir = new DirectoryInfo(@"profiles");
            DirectoryInfo[] Ammount = ProfDir.GetDirectories();
            Console.WriteLine(" ┌───┬─────────────────────────┬────────────────────┐");
            Console.WriteLine(" │ № │       Имя игрока        │     Рекорд         │");
            Console.WriteLine(" ├───┼─────────────────────────┼────────────────────┤");
            for (int i = 0; i < Ammount.Length; i++)
            {
                Console.WriteLine(" │{0,3}│{1,25}│{2,20}│", i + 1, GetTable.ReadLine(), GetTable.ReadLine());
                if (i != Ammount.Length - 1) Console.WriteLine(" ├───┼─────────────────────────┼────────────────────┤");
                else Console.WriteLine(" └───┴─────────────────────────┴────────────────────┘");
            }
            GetTable.Close();
            Console.Write(" * Нажмите любую клавишу, чтобы перейти в главное меню.\n > ");
            Console.ReadKey();
            Console.Clear();
            TypeTitle();
            MainMenu();
        }

        static void StartGame()
        //начало игры, выбор режима
        {
            Console.WriteLine(" * Выберите режим игры.");
            Console.Write(" 1) Аркада.\n 2) Тренировка. \n 3) Соревнование. \n 4) Перейти в меню. \n * Введите соответствующую цифру.\n > ");
            int control = 0;
            ControlInput(out control);
            Console.Clear();
            TypeTitle();
            switch (control)
            {
                case 1: currentsave = ""; ArcadeStart(); break;
                case 2: Training(SetDistance(), SetHole());
                    break;
                case 3: CompetitionMenu(); break;
                case 4: MainMenu(); break;
                default: Console.WriteLine(" * Некорректный ввод.");
                    MainMenu();
                    break;
            }
        }

        static void HelpMenu()
        //вывод описания игры и режимов
        {
            int control;
            Console.WriteLine(" * Выберите нужный вариант.");
            Console.Write(" 1) Описание игры.\n 2) Описание режима \"Аркада\"\n 3) Описание режима \"Тренировка\"\n 4) Описание режима \"Соревнование\"\n 5) Перейти в главное меню\n");
            Console.Write(" * Введите соответствующую цифру.\n > ");
            ControlInput(out control);
            string st = "";
            Console.Clear();
            TypeTitle();
            switch (control)
            {
                case 1: st = File.ReadAllText(@"help/game_help.txt"); break;
                case 2: st = File.ReadAllText(@"help/arcade_help.txt"); break;
                case 3: st = File.ReadAllText(@"help/training_help.txt"); break;
                case 4: st = File.ReadAllText(@"help/competition_help.txt"); break;
                case 5: Console.Clear(); TypeTitle(); MainMenu(); break;
                default: Console.Clear(); Console.WriteLine(" * Ошибка."); TypeTitle(); MainMenu(); break;
            }
            Console.WriteLine(st);
            Console.Write(" * Нажмите любую клавишу.\n > ");
            Console.ReadKey();
            Console.Clear();
            TypeTitle();
            HelpMenu();
        }

        static int SetDistance()
        //генерирует расстояние до лунки
        {
            Random rnd = new Random();
            return rnd.Next(60, 301);
        }

        static int SetHole()
        //генерирует ширину лунки
        {
            Random rnd = new Random();
            return rnd.Next(2, 11);
        }

        static double Range(double angle, double force)
        //считает точку приземления по X
        {
            double a = (force * force * Math.Sin(2 * angle * Math.PI / 180)) / 10;
            return a;
        }

        static int RandomAngle()
        //добавляет углу случайности
        {
            Random rnd = new Random();
            return rnd.Next(25, 61);
        }

        static void MainHint()
        //подсказка для удобного счета на калькуляторе
        {
            FileInfo typename = new FileInfo(profile);
            string st1 = typename.Name;
            string st2 = typename.Extension;
            st1 = st1.Substring(0, st1.Length - st2.Length);
            Console.WriteLine();
            Console.WriteLine(" * Имя профиля: {0}", st1);
            Console.WriteLine(" * g = 10");
            Console.WriteLine(" * Формула: L=(V^2 * sin(2 * a))/(g)");
            Console.WriteLine(" * Сила удара: V=sqrt((L * g)/(sin(2 * a))");
            Console.WriteLine();
        }

        static void ArcadeStart()
        //начинать новую игру или загрузить старую
        {
            Console.Clear();
            TypeTitle();
            Console.WriteLine(" * Выбериет вариант.");
            Console.WriteLine(" 1) Начать новую игру.\n 2) Загрузить сохраненную игру.\n 3) Перейти в меню.");
            Console.Write(" * Введите соответствующую цифру.\n > ");
            int control;
            ControlInput(out control);
            Console.Clear();
            TypeTitle();
            switch (control)
            {
                case 1: score = 0; streak = 0; lifes = 5; turn = 0; Arcade(); break;
                case 2: LoadGame(); break;
                case 3: MainMenu(); break;
                default: Console.WriteLine(" * Некорректный ввод."); MainMenu(); break;
            }
        }

        static void Arcade()
        //игра с набором очков
        {
            MainHint();
            Console.WriteLine(" * У вас осталось {0} жизней.", lifes);
            Console.WriteLine(" * Совершено {0} ходов. Осталось {1} ходов.", turn, 15-turn);
            double angle;
            double force;
            int distance = SetDistance();
            int hole = SetHole();
            Console.WriteLine(" * Расстояние до лунки {0}.", distance);
            Console.WriteLine(" * Ширина лунки {0}.", hole);
            DrawLandscape(distance, hole, 0, 0, false);
            Console.Write(" * Угол удара.\n > ");
            angle = (double)RandomAngle();
            Console.WriteLine(angle);
            Console.Write(" * Введите силу удара.\n > ");
            DigitInput(out force);
            double landing = Range(angle, force);
            Console.WriteLine(" * Мяч пролетел {0} метров.", (int)landing);
            DrawLandscape(distance, hole, angle, force, true);
            ArcadeResult(distance, hole, landing);
            turn++;
            if (lifes == 0 || turn >= 15)
            {
                Console.WriteLine(" * Игра окончена! Всего вы набрали {0} очков.", score);
                ArcadeSave();
                if (currentsave != "") DeleteSave();
                RefreshHighscoreTable();
                Console.Write(" * Результат сохранен. Нажмите любую клавишу, чтобы перейти в главное меню.\n > ");
                Console.ReadKey();
                Console.Clear();
                TypeTitle();
                MainMenu();
            }
            else ArcadeMenu();
        }

        static void ArcadeResult(double dist, double hole, double landing)
        //просчитывает результат аркады
        {
            if (landing >= (dist - hole / 2) && landing <= (dist + hole / 2))
            {
                VictorySound();
                Console.WriteLine(" # Попадание в лунку! Поздравляем!");

                streak++;
                if (streak >= 2)
                {
                    Console.WriteLine(" * {0} попадания подряд! +{1} очков!", streak, streak * 10);
                    score += streak * 10;
                    if (streak == 5)
                    {
                        Console.WriteLine(" * Великолепно!");
                        RampageSound();
                        if (lifes < 10)
                        {
                            Console.WriteLine(" * Дополнительная жизнь!");
                            lifes++;
                        }
                    }
                }

                if (landing == dist)
                {
                    Console.WriteLine(" * В ЯБЛОЧКО! +150 ОЧКОВ!");
                    score += 150;
                }
                else

                    if (landing >= (dist + (hole * 1 / 3)) || landing <= (dist - (hole * 1 / 3)))
                    {
                        Console.WriteLine(" * В центральную часть! +100 очков!");
                        score += 100;
                    }
                    else

                        if (landing < (dist + (hole * 1 / 3)) || landing > (dist - (hole * 1 / 3)))
                        {
                            Console.WriteLine(" * В крайнюю часть! +50 очков!");
                            score += 50;
                        }
                        else

                            if (landing == (dist - hole / 2) || landing == (dist + hole / 2))
                            {
                                Console.WriteLine(" * Удача! Точно в край! +60 очков!");
                                score += 60;
                            }


            }
            else
            {
                DefeatSound();
                Console.WriteLine(" # Промах! Не отчаивайтесь, в следующий раз получится!");
                if (score >= 30)
                {
                    Console.WriteLine(" * -30 очков.");
                    score -= 30;
                }
                else
                {
                    Console.WriteLine(" * -30 очков.");
                    score = 0;
                }
                Console.WriteLine(" * Количество попаданий подряд обнулено.");
                streak = 0;
                if (lifes != 0) lifes--;
            }
            Console.WriteLine(" * У вас {0} очков.", score);
        }

        static void ArcadeMenu()
        //меню аркады
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            Console.WriteLine(" * Выберите действие.");
            int control = 0;
            bool t = false;
            while (!t)
            {
                t = false;
                Console.WriteLine(" 1) Отправиться к следующей лунке.\n 2) Перейти в главное меню.");
                Console.Write(" * Введите соответствующую цифру.\n > ");
                ControlInput(out control);
                Console.Clear();
                TypeTitle();
                switch (control)
                {
                    case 1: player.Stop(); Arcade(); break;
                    case 2: player.Stop(); SaveGame(); Console.WriteLine(" * Вы набрали {0} очков. Результат сохранен.\n", score); MainMenu(); break;
                    default: Console.WriteLine(" * Некорректный ввод."); t = true; ArcadeMenu(); break;
                }
            }
        }

        static void Training(int distance, int hole)
        //режим, чтобы научиться попадать
        {
            double angle;
            double force;
            bool repeat = true;
            while (repeat)
            {
                MainHint();
                Console.WriteLine(" * Расстояние до лунки {0} метров.", distance);
                Console.WriteLine(" * Ширина лунки {0} метров.", hole);
                DrawLandscape(distance, hole, 0, 0, false);
                Console.Write(" * Введите угол удара.\n > ");
                DigitInput(out angle);
                Console.Write(" * Введите силу удара.\n > ");
                DigitInput(out force);
                double landing = Range(angle, force);
                Console.Clear();
                TypeTitle();
                Console.WriteLine(" * Мяч пролетел {0} метров.", (int)landing);
                DrawLandscape(distance, hole, angle, force, true);
                TrainingResult(distance, hole, landing);
                Console.WriteLine(" * Выберите действие.");
                int control = 0;
                bool t = true;
                while (t)
                {
                    t = false;
                    Console.WriteLine(" 1) Повторить бросок.\n 2) Новый бросок\n 3) Перейти в главное меню.");
                    Console.Write(" * Введите соответствующую цифру.\n > ");
                    ControlInput(out control);
                    Console.Clear();
                    TypeTitle();
                    switch (control)
                    {
                        case 1: Console.WriteLine(" * Новая попытка."); repeat = true; break;
                        case 2: Console.WriteLine(" * Переход к новой лунке."); Training(SetDistance(), SetHole()); break;
                        case 3: repeat = false; MainMenu(); break;
                        default: Console.WriteLine(" * Некорректный ввод."); t = true; break;
                    }
                }
            }
        }

        static void TrainingResult(double dist, double hole, double landing)
        //просчитывает результат тренировки
        {
            if (landing >= (dist - hole / 2) && landing <= (dist + hole / 2))
            {
                VictorySound();
                Console.WriteLine(" # Попадание в лунку! Поздравляем!");
            }
            else
            {
                DefeatSound();
                Console.WriteLine(" # Промах! Не отчаивайтесь, в следующий раз получится!");
            }
        }

        static void CompetitionMenu()
        //настройка соревновательного режима
        {
            Console.WriteLine(" * Добро пожаловать в соревновательный режим!");
            bool t = true;
            int number = 0;
            while (t)
            {
                Console.Write(" * Введите количество участников соревнования.\n > ");
                ControlInput(out number);
                DirectoryInfo ProfDir = new DirectoryInfo(@"profiles");
                DirectoryInfo[] CheckAmmount = ProfDir.GetDirectories();
                if (number <= CheckAmmount.Length && number > 1) t = false;
                else
                {
                    Console.WriteLine(" # Введенное вами число больше количества профилей({0}).", CheckAmmount.Length);
                    t = true;
                    Console.WriteLine(" * Выйти в главное меню? | 1 - Да | 2 - Нет |");
                    Console.Write(" > ");
                    int control;
                    ControlInput(out control);
                    Console.Clear();
                    TypeTitle();
                    if (control == 1) MainMenu();
                }
            }
            string[] players = new string[number];
            Console.WriteLine(" * Игроки по очереди выбирают профиль.");
            Console.WriteLine(" * Один и тот же профиль не может быть выбран несколько раз!");
            players[0] = profile;
            Console.Clear();
            TypeTitle();
            Console.WriteLine(" * Игроку 1 назначен профиль {0}.", profile);
            for (int i = 1; i < number; i++)
            {
                bool s = true;
                while (s)
                {
                    Console.WriteLine(" * Игрок {0} выбирает свой профиль", i + 1);
                    SelectProfile();
                    bool f = false;
                    for (int j = 0; j < i; j++)
                        if (players[j] == profile) f = true;
                    if (!f) players[i] = profile;
                    s = f;
                    Console.Clear();
                    TypeTitle();
                }
            }
            Console.WriteLine(" * Итоговый список участников:");
            for (int i = 0; i < number; i++)
                Console.WriteLine(" {0}) {1}", i + 1, players[i]);
            Console.WriteLine(" * ВНИМАНИЕ! Отсутствует возможность сохранения во время соревнования!");
            Console.WriteLine(" * После окончания соревнования результаты будут сохранены.");
            Console.Write(" * Нажмите любую клавишу, чтобы начать соревнование.\n > ");
            Competition(players);
        }

        static void Competition(string[] players)
        //соревновательный режим на несколько человек
        {
            int[,] strikes = new int[players.Length, 3];
            int distance, hole;
            double angle, force;
            for (int tu = 0; tu < 3; tu++)
            {
                distance = SetDistance();
                hole = SetHole();
                for (int i = 0; i < players.Length; i++)
                {
                    int dist = distance;
                    Console.Clear();
                    TypeTitle();
                    profile = players[i];
                    MainHint();
                    bool t = true;
                    int shots = 0;
                    while (t)
                    {
                        Console.WriteLine(" * Расстояние до лунки {0} метров.", dist);
                        Console.WriteLine(" * Ширина лунки {0} метров.", hole);
                        DrawLandscape(dist, hole, 0, 0, false);
                        Console.Write(" * Введите угол удара.\n > ");
                        DigitInput(out angle);
                        Console.Write(" * Введите силу удара.\n > ");
                        DigitInput(out force);
                        double landing = Range(angle, force);
                        shots++;
                        if (landing >= (dist - hole / 2) && landing <= (dist + hole / 2))
                        {
                            Console.WriteLine(" # Попадание в лунку! Поздравляем!");
                            t = false;
                            strikes[i, tu] = shots;
                            Console.Write(" * Ход следующего игрока. Нажмите любую клавишу, чтобы начать.\n > ");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.Clear();
                            TypeTitle();
                            MainHint();
                            Console.WriteLine(" # Промах! Подойдите к мячу и бейте снова! Вперёд!");
                            t = true;
                            dist = (int)Math.Abs(dist - landing);
                        }
                        Console.WriteLine(" * Мяч пролетел {0} метров.", (int)landing);
                        DrawLandscape(dist, hole, angle, force, true);
                    }
                }
            }
            Console.Clear();
            TypeTitle();
            Console.WriteLine(" * Результаты соревнования:");
            int max = 0;
            int winner = -1;
            for (int i = 0; i < players.Length; i++)
            {
                Console.Write("{0}: ", players[i]);
                int x = 0;
                for (int j = 0; j < 3; j++)
                    x += 100 / strikes[i, j];
                strikes[i, 0] = x;
                Console.WriteLine(x);
                if (x >= max)
                {
                    max = x;
                    winner = i;
                }
            }
            profile = players[0];
            Console.WriteLine(" # {0} становится победителем! Поздравляю!", players[winner]);
            CompetitionSave(players, strikes);
            Console.Write(" * Результаты сохранены. Нажмите любую клавишу, чтобы перейти в главное меню.\n > ");
            Console.ReadKey();
            Console.Clear();
            TypeTitle();
            MainMenu();
        }

        static void ProfileMenu()
        //управление профилями
        {
            bool t = true;
            while (t)
            {
                Console.WriteLine(" * Управление профилями.");
                Console.WriteLine(" 1) Создать новый профиль.\n 2) Выбрать существующий профиль.\n 3) Выйти из игры.");
                Console.Write(" * Выберите действие.\n > ");
                int control = 0;
                t = false;
                ControlInput(out control);
                Console.Clear();
                TypeTitle();
                switch (control)
                {
                    case 1: CreateProfile(); break;
                    case 2: Console.WriteLine(" * Текущий профиль: {0}.", (profile == "") ? "не выбран" : profile); SelectProfile(); ControlProfile(); break;
                    case 3: GameQuit(); break;
                    default: Console.WriteLine(" * Некорректный ввод."); t = true; break;
                }
            }
        }

        static void CreateProfile()
        //создать новый профиль
        {
            string newname = "";
            bool t = true;
            while (t)
            {
                Console.Write(" * Введите имя профиля.\n > ");
                newname = Console.ReadLine();
                if (newname.ToLower() != "con")
                {
                    char[] c = { '*', '|', '\\', ':', '"', '<', '>', '?', '/' };
                    t = false;
                    int i = 0;
                    while (!t && i < newname.Length)
                    {
                        if (Array.IndexOf(c, newname[i]) != -1) t = true;
                        i++;
                    }
                }
                else Console.WriteLine(" * Имя файла не должно содержать буквосочетания CON.");
                if (t) Console.WriteLine(" * Имя профиля не должно содержать символов *, |, \\, :, \", <, >, ?, /.");
            }
            DirectoryInfo NewProfile = new DirectoryInfo(@"profiles\" + newname);
            t = true;
            while (t)
            {
                NewProfile = new DirectoryInfo(@"profiles\" + newname);
                if (NewProfile.Exists == true)
                {
                    t = true;
                    Console.WriteLine(" * Профиль с таким именем уже существует.");
                    Console.WriteLine(" * Выберите нужное действие.\n 1) Ввести другое имя профиля.\n 2) Перейти в меню профилей.");
                    Console.Write(" * Введите соответствующую цифру.\n > ");
                    int control;
                    ControlInput(out control);
                    switch (control)
                    {
                        case 1: Console.Write(" * Введите другое имя профиля.\n > "); break;
                        default: ProfileMenu(); break;
                    }
                    newname = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine(" # Профиль \"{0}\" успешно создан.", newname);
                    t = false;
                }
            }
            NewProfile.Create();
            FileInfo FillArcade = new FileInfo(@"profiles\" + newname + "\\" + newname + "_arcade.txt");
            StreamWriter arc = FillArcade.CreateText();
            arc.Close();
            FileInfo FillComp = new FileInfo(@"profiles\" + newname + "\\" + newname + "_competition.txt");
            StreamWriter sav = FillComp.CreateText();
            sav.Close();
            profile = newname;
            Console.Write(" * Нажмите любую клавишу, чтобы перейти в меню.\n > ");
            Console.ReadKey();
            Console.Clear();
            TypeTitle();
            WelcomeSign();
            MainMenu();
        }

        static void SelectProfile()
        //выбрать существующий профиль
        {
            DirectoryInfo profiledir = new DirectoryInfo(@"profiles\");
            DirectoryInfo[] profiles = profiledir.GetDirectories();
            if (profiles.Length == 0)
            {
                Console.Clear();
                TypeTitle();
                Console.WriteLine(" * Нет профилей, доступных для выбора. Создайте новый профиль.");
                ProfileMenu();
            }
            Console.WriteLine(" * Выберите профиль.");
            for (int i = 0; i < profiles.Length; i++)
                Console.WriteLine(" {0}) Имя: {1}; Дата и время последней игры: {2};", i + 1, profiles[i].Name, profiles[i].LastWriteTime);
            Console.Write(" * Введите соответствующую цифру.\n > ");
            int control;
            ControlInput(out control);
            Console.Clear();
            TypeTitle();
            if (control <= profiles.Length && control >= 1)
                profile = profiles[control - 1].ToString();
            else
            {
                Console.WriteLine(" * Некорректный ввод.");
                ProfileMenu();
            }
        }

        static void ControlProfile()
        //управление профилем
        {
            bool t = true;
            while (t)
            {
                t = false;
                Console.WriteLine(" * Выберите действие:");
                Console.WriteLine(" 1) Перейти в меню.\n 2) Удалить профиль.\n");
                Console.Write(" * Введите соответствующую цифру.\n > ");
                int control;
                ControlInput(out control);
                switch (control)
                {
                    case 1: break;
                    case 2: Console.Write(" # Вы точно хотите удалить профиль? >> | Введите 9 если хотите |\n > ");
                        string ask = Console.ReadLine();
                        if (ask == "9")
                        {
                            DirectoryInfo delprofile = new DirectoryInfo(@"profiles\\" + profile);
                            delprofile.Delete(true);
                            profile = "";
                            Console.WriteLine(" * Профиль удален.");
                        }
                        Console.Write(" * Нажмите любую кнопку для перехода в меню.\n > ");
                        Console.ReadKey();
                        Console.Clear();
                        TypeTitle();
                        ProfileMenu();
                        break;
                    default: Console.WriteLine(" * Некорректный ввод.");
                        t = true;
                        break;
                }
            }
            Console.Clear();
            TypeTitle();
            WelcomeSign();
            MainMenu();
        }

        static void SaveGame()
        //сохраняет результат
        {
            if (currentsave != "")
                DeleteSave();
            DirectoryInfo SaveDir = new DirectoryInfo(@"profiles\\" + profile + "\\");
            FileInfo[] SaveList = SaveDir.GetFiles();
            string name = profile + "_save_" + Convert.ToString(SaveList.Length - 1);
            FileInfo NewSave = new FileInfo(@"profiles\\" + profile + "\\" + name + ".txt");
            StreamWriter CreateSave = NewSave.CreateText();
            Console.Write(" * Введите название сохранения.\n > ");
            CreateSave.WriteLine(Console.ReadLine());
            CreateSave.WriteLine(score);
            CreateSave.WriteLine(lifes);
            CreateSave.WriteLine(streak);
            CreateSave.WriteLine(turn);
            CreateSave.Close();
        }

        static void DeleteSave()
        //удаляет старое сохранение во избежание жульничества
        {
            FileInfo DelSave = new FileInfo(currentsave);
            DelSave.Delete();
        }

        static void LoadGame()
        //загрузка сохраненной игры
        {
            Console.WriteLine(" * Список сохранений:\n * <Имя>    <Набранные очки> <Оставшиеся жизни> <Дата и время сохранения>");
            DirectoryInfo LoadDir = new DirectoryInfo(@"profiles\\" + profile + "\\");
            FileInfo[] SaveListTmp = LoadDir.GetFiles();
            FileInfo[] SaveList = new FileInfo[SaveListTmp.Length - 2];
            for (int i = 0; i < SaveList.Length; i++)
                SaveList[i] = SaveListTmp[2 + i];
            if (SaveList.Length != 0)
            {
                for (int i = 0; i < SaveList.Length; i++)
                {
                    StreamReader WriteSaveInfo = new StreamReader(SaveList[i].FullName);
                    Console.Write(" {0,2}) {1,-20} {2,-4} {3,-2} {4}", i + 1, WriteSaveInfo.ReadLine(), WriteSaveInfo.ReadLine(), WriteSaveInfo.ReadLine(), SaveList[i].LastWriteTime);
                    WriteSaveInfo.Close();
                    Console.WriteLine();
                }
                Console.Write(" * Введите номер нужного сохранения.\n > ");
                int control;
                ControlInput(out control);
                control = Convert.ToInt32(control);
                Console.Clear();
                TypeTitle();
                if (control >= 1 && control <= SaveList.Length)
                {
                    StreamReader GetSaveInfo = new StreamReader(SaveList[control - 1].FullName);
                    string st = GetSaveInfo.ReadLine();
                    score = Convert.ToInt32(GetSaveInfo.ReadLine());
                    lifes = Convert.ToInt32(GetSaveInfo.ReadLine());
                    streak = Convert.ToInt32(GetSaveInfo.ReadLine());
                    turn = Convert.ToInt32(GetSaveInfo.ReadLine());
                    GetSaveInfo.Close();
                }
                else
                {

                    Console.WriteLine(" * Некорректный ввод.\n");
                    MainMenu();
                }
                currentsave = SaveList[control - 1].FullName;
                Arcade();
            }
            else
            {
                Console.WriteLine(" * Нет доступных сохранений. Начните новую игру.");
                StartGame();
            }
        }

        static void ArcadeSave()
        //сохраняет очки по окончанию сессии аркады
        {
            FileInfo RecordFile = new FileInfo(@"profiles\" + profile + "\\" + profile + "_arcade.txt");
            StreamReader ReadRecord = RecordFile.OpenText();
            int tmp = Convert.ToInt32(ReadRecord.ReadLine());
            ReadRecord.Close();
            if (score >= tmp)
            {
                //RecordFile.Delete();
                //RecordFile.Create();
                StreamWriter WriteRecord = RecordFile.CreateText();
                WriteRecord.WriteLine(score);
                WriteRecord.Close();
            }
        }

        static void CompetitionSave(string[] players, int[,] results)
        //сохраняет результаты соревнования
        {
            for (int i = 0; i < players.Length; i++)
            {
                FileInfo RecordFile = new FileInfo(@"profiles\" + players[i] + @"\" + players[i] + "_competition");
                StreamReader ReadRecord = RecordFile.OpenText();
                int tmp = Convert.ToInt32(ReadRecord.ReadLine());
                ReadRecord.Close();
                if (results[i, 0] >= tmp)
                {
                    RecordFile.Delete();
                    RecordFile.Create();
                    StreamWriter WriteRecord = RecordFile.AppendText();
                    WriteRecord.Write(results[i, 0]);
                    WriteRecord.Close();
                }
            }
        }

        static void DrawPoints(ref int left, int stepx,ref int x, ref int right, ref int points, ref int curpos, int space, int center)
        {
            if (points > 0)
            {
                left = 15 + center - x;
                if (left < 80)
                {
                    Console.SetCursorPosition(left, curpos + space);
                    Console.Write("☼");
                }
                right = 15 + center + x;
                if (right < 80)
                {
                    Console.SetCursorPosition(right, curpos + space);
                    Console.WriteLine("☼");
                }
                else Console.WriteLine();
                curpos++;
            }
            points--;
            x += stepx;
        }

        static void DrawLandscape(int distance, int hole, double angle, double force, bool draw)
        //отображает землю и игрока
        {
            int newdist = distance / 5;
            int newhole = hole / 5 + 1;
            double l = Range(angle, force);
            int newl = (int)l / 5;
            int left = 0;
            int right = 0;
            double h = (force * force * Math.Sin(angle * Math.PI / 180) * Math.Sin(angle * Math.PI / 180)) / 20;
            int points = (int)h / 20;
            int space = 0;
            bool y = false;
            if (points >= 8)
            {
                y = true;
                space = 0;
                points = 8;
            }
            else if (points <= 1)
            {
                space = 6;
                points = 1;
            }
            else space = 8 - points - 1;
            int stepx = (newl / (2*points));
            if (stepx == 0) stepx = 1;
            int x = stepx;
            int center = points*stepx;
            int curpos = Console.CursorTop;
            if (curpos <= 0) curpos = 1;
            points++;
            int tmp = 1;
            Console.WriteLine("               ");
            tmp = Console.CursorTop;
            if (draw)
            {
                if (points > 0 && points <= 8)
                {
                    left = 15 + center;
                    if (left < 80)
                    {
                        Console.SetCursorPosition(left, curpos+space);
                        Console.WriteLine("☼");
                    }
                    else Console.WriteLine();
                    curpos++;
                }
                points--;
            }
            else Console.WriteLine();
            Console.SetCursorPosition(0, tmp);
            Console.WriteLine(" ▄▄███▄▄       ");
            tmp = Console.CursorTop;
            if (draw)
            {
                if (y) curpos--;
                DrawPoints(ref left, stepx, ref x, ref right, ref points, ref curpos, space, center);
            }
            else Console.WriteLine();
            Console.SetCursorPosition(0, tmp);
            Console.WriteLine("  ▐▄█▄▌        ");
            tmp = Console.CursorTop;
            if (draw)
                DrawPoints(ref left, stepx, ref x, ref right, ref points, ref curpos, space, center);
            else Console.WriteLine();
            Console.SetCursorPosition(0, tmp);
            Console.WriteLine(" ▄▄▄█▄▄▄       ");
            tmp = Console.CursorTop;
            if (draw)
                DrawPoints(ref left, stepx, ref x, ref right, ref points, ref curpos, space, center);
            else Console.WriteLine();
            Console.SetCursorPosition(0, tmp);
            Console.WriteLine(" █▐███▌█       ");
            tmp = Console.CursorTop;
            if (draw)
            {
                if (y) curpos++;
                DrawPoints(ref left, stepx, ref x, ref right, ref points, ref curpos, space, center);
            }
            else Console.WriteLine();
            Console.SetCursorPosition(0, tmp);
            Console.WriteLine(" █▐███▌█▌      ");
            tmp = Console.CursorTop;
            if (draw)
                DrawPoints(ref left, stepx, ref x, ref right, ref points, ref curpos, space, center);
            else Console.WriteLine();
            Console.SetCursorPosition(0, tmp);
            Console.WriteLine("   █ █  ▌      ");
            tmp = Console.CursorTop;
            if (draw)
                DrawPoints(ref left, stepx, ref x, ref right, ref points, ref curpos, space, center);
            Console.SetCursorPosition(15 + newhole + newdist, tmp-1);
            Console.Write("▐█");
            Console.SetCursorPosition(0, tmp);
            Console.WriteLine("  ▄█ █▄ ▀▀    ▄");
            tmp = Console.CursorTop;
            if (draw)
                DrawPoints(ref left, stepx, ref x, ref right, ref points, ref curpos, space, center);
            Console.SetCursorPosition(15 + newhole + newdist, tmp-1);
            Console.Write("▐");

            if (draw)
            {
                Console.SetCursorPosition(12, tmp - 2);
                Console.Write("←{0}→", (int)l);
                Console.SetCursorPosition(14, tmp - 1);
                Console.Write(" ");
                if (right < 80)
                {
                    Console.SetCursorPosition(right, tmp - 1);
                    Console.Write("▄");
                }
            }

            Console.SetCursorPosition(0, tmp);
            for (int i = 1; i <= 80; i++)
                if (i > newdist + 15 && i <= newdist + newhole + 15) Console.Write("▄");
                else Console.Write("▓");

            Console.SetCursorPosition(14 + newdist / 2, Console.CursorTop);
            Console.Write("←{0}→", distance);
            Console.SetCursorPosition(14 + newdist, Console.CursorTop);
            Console.WriteLine("→{0," + newhole + "}←", hole);

        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Title = "The Epic Golf!!!";
            StartScreen();
            TypeTitle();
            SoundControl();
            MainSound();
            ProfileMenu();
        }
    }
}