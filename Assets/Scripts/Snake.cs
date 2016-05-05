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
    //Сторона движения
    public int Course,
    //Длина змеи
               Lenght;
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
            //colli,
            //colli2;
    public Collider[] colliders;
    //Конструктор----------------------------

    Snake()

    {
        //point = Head.transform.position;
        //настоящее время, после которого совершается ход
        Postime = 0;
        //Сторона движения
        Course = 0;
        //Длина змеи
        Lenght = 1;
        //Интервал между шагами
        Inteval = 0.5F;
        //Пауза
        Pause = false;
        //Змея не поела
        eating = false;
        //Список с частями хвоста
        tail = new List<Transform>();
        //Координаты перемещения головы----------------------

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
        //go = true;
        //colli = new Vector3(100, 100, 100);
        //colli2 = new Vector3(1, 1, 1);
        //colliders = Physics.OverlapBox(colli2, colli);

    }
    //-----------------------------------------

    //Передвижение
    public void Move()
    {
        //colliders[0].SendMessage("OVER LAP");
        //print(colliders[0]);
        if (eating)
        {   //Создание объекта тела
             GameObject g = (GameObject)Instantiate(Body, empty, Quaternion.identity);
            //добавление в список
            tail.Insert(0, g.transform);
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
            case 0: point = new Vector3(0, 0, 36); break;
            //Вниз
            case 1: point = new Vector3(0, 0, -36); ; break;
            //Влево
            case 2: point = new Vector3(-36, 0, 0); ; break;
            //Вправо
            case 3: point = new Vector3(36, 0, 0); ; break;
        }
        
        //прошёл ли интервал, не стоит ли пауза
        if (Time.time >= Postime && Pause)
        {
            empty = Head.transform.position;
            //расчёт времени
            Postime = Time.time + Inteval;
            //перемещение
            Head.transform.Translate(point);
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
