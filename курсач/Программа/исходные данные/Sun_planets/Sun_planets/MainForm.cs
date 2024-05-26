using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace Sun_planets
{
    public partial class MainForm : Form
    {
        int tex1, tex2, tex3, tex4, tex5, tex6, tex60, tex7, tex8, tex9, tex10; // текстуры
        double ang1, ang2, ang3, ang4, ang5, ang6, ang7, ang8; // углы поворота
        double moonOrbitAngle; // угол вращения Луны вокруг Земли
        double speedMultiplier = 1.0; // множитель скорости вращения планет

        // Переменные камеры
        double camX = 0, camY = 0, camZ = -20; // Позиция камеры
        double camRotX = 10, camRotY = 0; // Вращение камеры

        // Хитбоксы для планет
        RectangleF[] hitboxes;

        public MainForm()
        {
            InitializeComponent();
            GlCtrl.InitializeContexts(); // инициализация вывода

            // Настройка кнопок поверх проекции
            Button btnIncreaseSpeed = new Button();
            btnIncreaseSpeed.Text = "Увеличить скорость";
            btnIncreaseSpeed.Location = new Point(10, 10);
            btnIncreaseSpeed.Click += BtnIncreaseSpeed_Click;
            this.Controls.Add(btnIncreaseSpeed);
            btnIncreaseSpeed.BringToFront();

            Button btnDecreaseSpeed = new Button();
            btnDecreaseSpeed.Text = "Уменьшить скорость";
            btnDecreaseSpeed.Location = new Point(150, 10);
            btnDecreaseSpeed.Click += BtnDecreaseSpeed_Click;
            this.Controls.Add(btnDecreaseSpeed);
            btnDecreaseSpeed.BringToFront();

            // Добавляем обработчик событий клавиатуры
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
            this.KeyPreview = true; // Включаем предварительный просмотр клавиатуры

            // Добавляем обработчик событий мыши
            GlCtrl.MouseClick += new MouseEventHandler(GlCtrl_MouseClick);

            // Инициализация хитбоксов
            hitboxes = new RectangleF[8];
        }

        private void BtnIncreaseSpeed_Click(object sender, EventArgs e)
        {
            speedMultiplier *= 1.1;
        }

        private void BtnDecreaseSpeed_Click(object sender, EventArgs e)
        {
            speedMultiplier *= 0.9;
        }

        private int CreateTexture(string filename)
        {
            if (filename != "")
            {
                Bitmap bmp = new Bitmap(filename); // загрузка изображения из файла
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb); // блокировать изображение от изменений
                int texn;
                Gl.glGenTextures(1, out texn); // генерация номера текстуры
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, texn); // создание текстуры
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
                Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, (int)Gl.GL_RGBA, bmp.Width, bmp.Height, 0, Gl.GL_BGRA_EXT, Gl.GL_UNSIGNED_BYTE, bmpData.Scan0);
                bmp.UnlockBits(bmpData); // разблокировать изображение
                return texn; // возврат идентификатора текстуры
            }
            else return 0;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tex1 = CreateTexture("1.jpg"); // загрузка текстур
            tex2 = CreateTexture("2.jpg");
            tex3 = CreateTexture("3.jpg");
            tex4 = CreateTexture("4.jpg");
            tex5 = CreateTexture("5.jpg");
            tex6 = CreateTexture("6.jpg");
            tex60 = CreateTexture("60.jpg");
            tex7 = CreateTexture("7.jpg");
            tex8 = CreateTexture("8.jpg");
            tex9 = CreateTexture("9.jpg");
            tex10 = CreateTexture("11.jpg");

            Glut.glutInit(); // инициализация библиотеки glut
            Glut.glutInitDisplayMode(Glut.GLUT_RGBA | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH); // двойной буфер и буфер глубины
            Gl.glViewport(0, 0, GlCtrl.Width, GlCtrl.Height); // задание области вывода
            Gl.glEnable(Gl.GL_DEPTH_TEST); // включение буфера глубины
            Gl.glEnable(Gl.GL_TEXTURE_2D); // включение текстурирования
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);
            Gl.glMatrixMode(Gl.GL_PROJECTION); // выбор матрицы проекции
            Gl.glLoadIdentity(); // загрузка единичной
            Glu.gluPerspective(60.0, (double)GlCtrl.Width / GlCtrl.Height, 0.1, 100); // параметры перспективы
            Gl.glMatrixMode(Gl.GL_MODELVIEW); // выбор матрицы модели
            Gl.glClearColor(0, 0, 0, 0); // цвет очистки экрана
            ang1 = 0;
            ang2 = 60;
            ang3 = 160;
            ang4 = 260;
            ang5 = 360;
            ang6 = 450;
            ang7 = 500;
            ang8 = 520;
            moonOrbitAngle = 0; // начальный угол вращения Луны вокруг Земли

            tmr.Start(); // старт анимации
        }

        void SolidSphere(double r, int nx, int ny)
        {
            int ix, iy;
            double x, y, z, sy, cy, sy1, cy1, sx, cx, piy, pix, ay, ay1, ax, tx, ty, ty1, dnx, dny, diy;
            dnx = 1.0 / (double)nx;
            dny = 1.0 / (double)ny;
            Gl.glColor3d(0.0, 0.0, 0.0);
            Gl.glBegin(Gl.GL_QUAD_STRIP); // сфера состоит из соединённых полигонов
            piy = Math.PI * dny;
            pix = Math.PI * dnx;
            for (iy = 0; iy < ny; iy++)
            {
                diy = (double)iy;
                ay = diy * piy;
                sy = Math.Sin(ay);
                cy = Math.Cos(ay);
                ty = diy * dny;
                ay1 = ay + piy;
                sy1 = Math.Sin(ay1);
                cy1 = Math.Cos(ay1);
                ty1 = ty + dny;
                for (ix = 0; ix <= nx; ix++)
                {
                    ax = 2.0 * ix * pix;
                    sx = Math.Sin(ax);
                    cx = Math.Cos(ax);
                    x = r * sy * cx;
                    y = r * sy * sx;
                    z = r * cy;
                    tx = (double)ix * dnx;
                    Gl.glTexCoord2d(tx, ty); // координаты текстуры в текущей вершине
                    Gl.glVertex3d(x, y, z); // координаты вершин полигона
                    x = r * sy1 * cx;
                    y = r * sy1 * sx;
                    z = r * cy1;
                    tx = (double)ix * dnx;
                    Gl.glTexCoord2d(tx, ty1); // координаты текстуры в текущей вершине
                    Gl.glVertex3d(x, y, z); // координаты вершин полигона
                }
            }
            Gl.glEnd();
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT); // очистка буферов вывода и глубины
            Gl.glLoadIdentity(); // загрузка единичной

            // Применение трансформаций камеры
            Gl.glRotated(camRotX, 1, 0, 0);
            Gl.glRotated(camRotY, 0, 1, 0);
            Gl.glTranslated(camX, camY, camZ);

            Gl.glPushMatrix();
            // вывод солнца
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, tex10); // выключение текстуры
            Gl.glColor3d(1, 0.949, 0);
            SolidSphere(1, 16, 16);
            // вывод орбит и планет
            Gl.glPushMatrix();
            DrawOrbit(4); // Орбита Меркурия (радиус 4)
            Gl.glRotated(ang1, 0, 1, 0);
            Gl.glTranslated(4, 0, 0);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, tex1); // выбор текстуры
            Gl.glRotated(-90, 1, 0, 0); // установка вертикально
            Gl.glRotated(ang1 * 5, 0, 0, 1); // вращение вокруг оси
            SolidSphere(0.3, 16, 16);
            // Обновление хитбокса для Меркурия
            UpdateHitbox(0, 4, ang1, 0.3);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            DrawOrbit(6); // Орбита Венеры (радиус 6)
            Gl.glRotated(ang2, 0, 1, 0);
            Gl.glTranslated(6, 0, 0);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, tex2); // выбор текстуры
            Gl.glRotated(-90, 1, 0, 0); // установка вертикально
            Gl.glRotated(ang2 * 3, 0, 0, 1); // вращение вокруг оси
            SolidSphere(0.35, 16, 16);
            // Обновление хитбокса для Венеры
            UpdateHitbox(1, 6, ang2, 0.35);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            DrawOrbit(8); // Орбита Земли (радиус 8)
            Gl.glRotated(ang3, 0, 1, 0);
            Gl.glTranslated(8, 0, 0);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, tex3); // выбор текстуры
            Gl.glRotated(-90, 1, 0, 0); // установка вертикально
            Gl.glRotated(ang3 * 4, 0, 0, 1); // вращение вокруг оси
            SolidSphere(0.4, 16, 16);
            // Обновление хитбокса для Земли
            UpdateHitbox(2, 8, ang3, 0.4);
            // Отрисовка Луны вокруг Земли
            Gl.glPushMatrix();
            Gl.glRotated(moonOrbitAngle, 0, 1, 0); // Угол вращения Луны вокруг Земли
            Gl.glTranslated(1, 0, 0); // Расстояние от Земли до Луны
            Gl.glRotated(ang2 * 3, 0, 1, 0); // Вращение Луны вокруг своей оси
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, tex9); // Выбор текстуры Луны
            SolidSphere(0.2, 16, 16); // Отрисовка Луны
            Gl.glPopMatrix();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            DrawOrbit(10); // Орбита Марса (радиус 10)
            Gl.glRotated(ang4, 0, 1, 0);
            Gl.glTranslated(10, 0, 0);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, tex4); // выбор текстуры
            Gl.glRotated(-90, 1, 0, 0); // установка вертикально
            Gl.glRotated(ang4 * 1.5, 0, 0, 1); // вращение вокруг оси
            SolidSphere(0.35, 16, 16);
            // Обновление хитбокса для Марса
            UpdateHitbox(3, 10, ang4, 0.35);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            DrawOrbit(13); // Орбита Юпитера (радиус 13)
            Gl.glRotated(ang5, 0, 1, 0);
            Gl.glTranslated(13, 0, 0);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, tex5); // выбор текстуры
            Gl.glRotated(-90, 1, 0, 0); // установка вертикально
            Gl.glRotated(ang5 * 1.2, 0, 0, 1); // вращение вокруг оси
            SolidSphere(0.9, 16, 16);
            // Обновление хитбокса для Юпитера
            UpdateHitbox(4, 13, ang5, 0.9);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            DrawOrbit(16); // Орбита Сатурна (радиус 16)
            Gl.glRotated(ang6, 0, 1, 0);
            Gl.glTranslated(16, 0, 0);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, tex6); // выбор текстуры
            Gl.glRotated(-90, 1, 0, 0); // установка вертикально
            Gl.glRotated(ang6 * 3, 0, 0, 1); // вращение вокруг оси
            SolidSphere(0.6, 16, 16);
            // Обновление хитбокса для Сатурна
            UpdateHitbox(5, 16, ang6, 0.6);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, tex60); // выбор текстуры
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(1, 1, 0);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3d(1, -1, 0);
            Gl.glTexCoord2d(1, 1);
            Gl.glVertex3d(-1, -1, 0);
            Gl.glTexCoord2d(0, 1);
            Gl.glVertex3d(-1, 1, 0);
            Gl.glEnd();
            Gl.glPopMatrix();

            Gl.glPopMatrix();

            ang1 += 2 * speedMultiplier;
            ang2 += 0.75 * speedMultiplier;
            ang3 += 0.5 * speedMultiplier;
            ang4 += 0.25 * speedMultiplier;
            ang5 += 0.125 * speedMultiplier;
            ang6 += 0.05 * speedMultiplier;
            ang7 += 0.05 * speedMultiplier;
            ang8 += 0.025 * speedMultiplier;
            moonOrbitAngle += 0.05 * speedMultiplier; // скорость вращения Луны

            GlCtrl.Invalidate();
        }

        private void DrawOrbit(double radius)
        {
            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glColor3d(1, 1, 1);
            for (int i = 0; i < 360; i++)
            {
                double degInRad = i * Math.PI / 180.0;
                Gl.glVertex3d(Math.Cos(degInRad) * radius, 0, Math.Sin(degInRad) * radius);
            }
            Gl.glEnd();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Gl.glViewport(0, 0, GlCtrl.Width, GlCtrl.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION); // выбор матрицы проекции
            Gl.glLoadIdentity(); // загрузка единичной
            Glu.gluPerspective(60.0, (double)GlCtrl.Width / GlCtrl.Height, 0.1, 100); // параметры перспективы
            Gl.glMatrixMode(Gl.GL_MODELVIEW); // выбор матрицы модели
        }

        private void GlCtrl_Paint(object sender, PaintEventArgs e)
        {
            // Необходимая логика для отрисовки в GlCtrl
            tmr_Tick(sender, e);
        }

        // Обработка нажатий клавиш для управления камерой
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            double moveSpeed = 1.0;
            double rotSpeed = 2.0;

            // Compute forward and right vectors based on camera rotation
            double radRotX = camRotX * Math.PI / 180.0;
            double radRotY = camRotY * Math.PI / 180.0;

            double forwardX = Math.Sin(radRotY);
            double forwardZ = Math.Cos(radRotY);
            double rightX = Math.Cos(radRotY);
            double rightZ = -Math.Sin(radRotY);
            double upY = -Math.Sin(radRotX);

            switch (e.KeyCode)
            {
                case Keys.W: // Движение вперёд
                    camX += forwardX * moveSpeed;
                    camZ += forwardZ * moveSpeed;
                    break;
                case Keys.S: // Движение назад
                    camX -= forwardX * moveSpeed;
                    camZ -= forwardZ * moveSpeed;
                    break;
                case Keys.A: // Движение влево
                    camX += rightX * moveSpeed;
                    camZ += rightZ * moveSpeed;
                    break;
                case Keys.D: // Движение вправо
                    camX -= rightX * moveSpeed;
                    camZ -= rightZ * moveSpeed;
                    break;
                case Keys.Q: // Движение вверх
                    camY += moveSpeed;
                    break;
                case Keys.E: // Движение вниз
                    camY -= moveSpeed;
                    break;
                case Keys.R: // Вращение влево
                    camRotY -= rotSpeed;
                    break;
                case Keys.Right: // Вращение вправо
                    camRotY += rotSpeed;
                    break;
                case Keys.T: // Вращение вверх
                    camRotX -= rotSpeed;
                    break;
                case Keys.Y: // Вращение вниз
                    camRotX += rotSpeed;
                    break;
            }

            // Обеспечение корректных значений углов вращения (0-360 градусов)
            camRotX = (camRotX + 360) % 360;
            camRotY = (camRotY + 360) % 360;
        }

        // Обновление хитбокса планеты
        private void UpdateHitbox(int index, double orbitRadius, double orbitAngle, double planetRadius)
        {
            double planetX = orbitRadius * Math.Cos(orbitAngle * Math.PI / 180);
            double planetZ = orbitRadius * Math.Sin(orbitAngle * Math.PI / 180);
            hitboxes[index] = new RectangleF((float)(planetX - planetRadius), (float)(planetZ - planetRadius), (float)(planetRadius * 2), (float)(planetRadius * 2));
        }

        // Обработчик клика по планете
        private void CheckPlanetClick(int mouseX, int mouseY)
        {
            for (int i = 0; i < hitboxes.Length; i++)
            {
                if (hitboxes[i].Contains(mouseX, mouseY))
                {
                    // Вызываем метод открытия формы с информацией о планете
                    ShowPlanetInfo(i);
                    break;
                }
            }
        }

        // Метод для открытия формы с информацией о планете
        private void ShowPlanetInfo(int planetIndex)
        {
            string name = "";
            string diameter = "";
            string distance = "";
            string description = "";

            switch (planetIndex)
            {
                case 0:
                    name = "Mercury";
                    diameter = "4,879 km";
                    distance = "57.91 million km";
                    description = "Mercury is the smallest planet in our solar system.";
                    break;
                case 1:
                    name = "Venus";
                    diameter = "12,104 km";
                    distance = "108.2 million km";
                    description = "Venus spins slowly in the opposite direction of most planets.";
                    break;
                case 2:
                    name = "Earth";
                    diameter = "12,742 km";
                    distance = "149.6 million km";
                    description = "Earth is the only planet known to support life.";
                    break;
                case 3:
                    name = "Mars";
                    diameter = "6,779 km";
                    distance = "227.9 million km";
                    description = "Mars is known as the Red Planet.";
                    break;
                case 4:
                    name = "Jupiter";
                    diameter = "139,820 km";
                    distance = "778.5 million km";
                    description = "Jupiter is the largest planet in our solar system.";
                    break;
                case 5:
                    name = "Saturn";
                    diameter = "116,460 km";
                    distance = "1.434 billion km";
                    description = "Saturn is famous for its rings.";
                    break;
                case 6:
                    name = "Uranus";
                    diameter = "50,724 km";
                    distance = "2.871 billion km";
                    description = "Uranus rotates on its side.";
                    break;
                case 7:
                    name = "Neptune";
                    diameter = "49,244 km";
                    distance = "4.495 billion km";
                    description = "Neptune is known for its intense blue color.";
                    break;
            }

            PlanetInfoForm infoForm = new PlanetInfoForm();
            infoForm.UpdateInfo(name, diameter, distance, description);
            infoForm.Show();
        }

        // Обработчик события клика мыши
        private void GlCtrl_MouseClick(object sender, MouseEventArgs e)
        {
            // Преобразование координат мыши в мировые координаты
            int[] viewport = new int[4];
            double[] modelview = new double[16];
            double[] projection = new double[16];
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
            Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, modelview);
            Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, projection);

            float winX = e.X;
            float winY = viewport[3] - e.Y;
            float[] winZ = new float[1];
            Gl.glReadPixels(e.X, (int)winY, 1, 1, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, winZ);

            double[] pos = new double[3];
            Glu.gluUnProject(winX, winY, winZ[0], modelview, projection, viewport, out pos[0], out pos[1], out pos[2]);

            CheckPlanetClick((int)pos[0], (int)pos[2]);
        }
    }
}