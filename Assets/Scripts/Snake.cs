using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Snake : MonoBehaviour {

    public GameObject Food;
    //Тело
    public GameObject Body; 
    //Голова
    public GameObject Head;
    //Поверхность событий
    public GameObject Float;
    //Сторона движения
    public int Course;
    //Горизонтальное или вертикальное движение
    public bool vh;
    //настоящее время, после которого совершается ход
    public float Postime,
    //Интервал между шагами
                 Inteval;
    //Пауза
    public bool Pause,
    //Поедание
                 eating;
    //Управление
    public int KeyPause, 
               KeyUp,
               KeyDown,
               KeyLeft,
               KeyRight;
    //Список хвоста
    List<Transform> tail;

    //Координаты
    Vector3 point,
            empty;
 
    public Collider[] colliders;
    //Конструктор----------------------------

    Snake()

    {

        //настоящее время, после которого совершается ход
        Postime = 0;
        //Сторона движения
        Course = 0;
        //Интервал между шагами
        Inteval = 0.5F;
        //Пауза
        Pause = false;
        //Змея не поела
        eating = false;
        //Список с частями хвоста
        tail = new List<Transform>();
        //Горизонтальное или вертикальное движение
        vh = true;

        //Пауза
        KeyPause = (int)KeyCode.Space;
        //Вверх
        KeyUp = (int)KeyCode.W;
        //Вниз
        KeyDown = (int)KeyCode.S;
        //Влево
        KeyLeft = (int)KeyCode.A;
        //Вправо
        KeyRight = (int)KeyCode.D;


    }
    //-----------------------------------------

    //Передвижение
    public void Move()
    {
        //активация поедания
        if (eating)
        {   //Создание объекта тела
             GameObject g = (GameObject)Instantiate(Body, empty, Quaternion.identity);
            //добавление в список
            tail.Insert(0, g.transform);
            //деактивация поедания
            eating = false;
        }
        //Если была нажата клавиша, то пауза
        if (Input.GetKey((KeyCode)KeyPause)) Pause = false;
        //Смена направления, деакцивация паузы
        if (Input.GetKey((KeyCode)KeyUp)) { Course = 0; Pause = true; }
        if (Input.GetKey((KeyCode)KeyDown)) { Course = 1; Pause = true; }
        if (Input.GetKey((KeyCode)KeyLeft)) { Course = 2; Pause = true; }
        if (Input.GetKey((KeyCode)KeyRight)) { Course = 3; Pause = true; }


        //направление
        switch (Course)
        {   //Движение вверх
            case 0: if (!Collide(0, -36, 36, Head, Float)) point = new Vector3(0, 0, 36); else Pause = false; break;
            //Вниз
            case 1: if (!Collide(0, -36, -36, Head, Float)) point = new Vector3(0, 0, -36); else Pause = false; break;
            //Влево
            case 2: if (!Collide(-36, -36, 0, Head, Float)) point = new Vector3(-36, 0, 0); else Pause = false; break;
            //Вправо
            case 3: if (!Collide(36, -36, 0, Head, Float)) point = new Vector3(36, 0, 0); else Pause = false; break;
        }
        
        //прошёл ли интервал, не стоит ли пауза
        if (Time.time >= Postime && Pause)
        {
            empty = Head.transform.position;
            //расчёт времени
            Postime = Time.time + Inteval;
            //перемещение
            Head.transform.Translate(point);
            //Движение хвоста
            if (tail.Count > 0)
            {
                // Move last Tail Element to where the Head was
                tail.Last().position = empty;
                // Добавить первый, удалить задний
                tail.Insert(0, tail.Last());
                tail.RemoveAt(tail.Count - 1);

            }

        }
        
        
    }

   public bool Collide(int x, int y, int z, GameObject item_check, GameObject item_collide)
    {
        //Координаты поверхности столкновения
        float planeup = item_collide.transform.position.y + ((item_collide.GetComponent<Renderer>().bounds.size.y) / 2);//Координаты перхней плоскости
        float planedown = item_collide.transform.position.y - ((item_collide.GetComponent<Renderer>().bounds.size.y) / 2);//Координаты нижней плоскости
        float planeright = item_collide.transform.position.x + ((item_collide.GetComponent<Renderer>().bounds.size.x) / 2);//Координаты правой плоскости
        float planeleft = item_collide.transform.position.x - ((item_collide.GetComponent<Renderer>().bounds.size.x) / 2);//Координаты левой плоскости
        float planefront = item_collide.transform.position.z - ((item_collide.GetComponent<Renderer>().bounds.size.z) / 2);//Координаты передней плоскости
        float planebehinde = item_collide.transform.position.z + ((item_collide.GetComponent<Renderer>().bounds.size.z) / 2);//Координаты задней лоскости

        //Координаты поверхности формального объекта
        float objectup = item_check.transform.position.y + y + ((item_check.GetComponent<Renderer>().bounds.size.y) / 2);//Координаты перхней плоскости
        float objectdown = item_check.transform.position.y + y - ((item_check.GetComponent<Renderer>().bounds.size.y) / 2);//Координаты нижней плоскости
        float objectright = item_check.transform.position.x + x + ((item_check.GetComponent<Renderer>().bounds.size.x) / 2);//Координаты правой плоскости
        float objectleft = item_check.transform.position.x + x - ((item_check.GetComponent<Renderer>().bounds.size.x) / 2);//Координаты левой плоскости
        float objectfront = item_check.transform.position.z + z - ((item_check.GetComponent<Renderer>().bounds.size.z) / 2);//Координаты передней плоскости
        float objectbehinde = item_check.transform.position.z + z + ((item_check.GetComponent<Renderer>().bounds.size.z) / 2);//Координаты задней лоскости

        
        if (planeup <= objectdown || objectup <= planedown)//Выше объект или ниже чем поверхность
            return true;
        else 
        if (planeleft >= objectright || objectleft >= planeright)//Левее объект или правее чем поверхность
            return true;
        else 
        if (planebehinde <= objectfront || objectbehinde <= planefront)//Дальше объект или ближе чем поверхность
            return true;
        else return false;


    } 
 
    //Вызов при столкновении
void OnTriggerEnter(Collider coll)
    {
        if (coll.name.StartsWith("FoodPrefab"))
        {
            Destroy(coll.gameObject);
            print("EAT!");
            eating = true;
        }
        if(coll.name.StartsWith("Body"))
        {
            print("!!!NOT EAT!!!");
        }
    }
    public bool spawn = false;
    void Update () {
        Move();
        if (spawn)
        {                 
            Instantiate(Food, new Vector3(72, 0, 72), Quaternion.identity);
            spawn = false;
        }
    }
    }
