using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{

    //переменная для установки макс. скорости персонажа
    public float maxSpeed = 8f;
    //переменная для определения направления персонажа вправо/влево
    private bool isFacingRight = true;
    //ссылка на компонент анимаций
    private Animator anim;
    private Rigidbody2D myRigidbody;
    public float move;
    public float posishn;

    //находится ли персонаж на земле или в прыжке?
    public bool isGrounded = false;
    //ссылка на компонент Transform объекта
    //для определения соприкосновения с землей
    public Transform groundCheck;
    //радиус определения соприкосновения с землей
    private float groundRadius = 0.2f;
    //ссылка на слой, представляющий землю
    public LayerMask whatIsGround;

    //находится ли персонаж на земле или в прыжке?
    public bool isFood = false;
    //ссылка на компонент Transform объекта
    //для определения соприкосновения с землей
    public Transform foodCheck;
    //радиус определения соприкосновения с землей
    private float foodRadius = 0.5f;
    //ссылка на слой, представляющий землю
    public LayerMask whatIsFood;

   SemkaController semkControl;

    /// <summary>
    /// Начальная инициализация
    /// </summary>
    private void Start()
    {
        anim = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();    
    }

    /// <summary>
    /// Выполняем действия в методе FixedUpdate, т. к. в компоненте Animator персонажа
    /// выставлено значение Animate Physics = true и анимация синхронизируется с расчетами физики
    /// </summary>
    private void FixedUpdate()
    {
        posishn = myRigidbody.velocity.y;
        //определяем, на земле ли персонаж
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        //устанавливаем соответствующую переменную в аниматоре
        anim.SetBool("Ground", isGrounded);
        //устанавливаем в аниматоре значение скорости взлета/падения
        anim.SetFloat("vSpeed", myRigidbody.velocity.y);
        //если персонаж в прыжке - выход из метода, чтобы не выполнялись действия, связанные с бегом
        //if (!isGrounded)
        //    return;
        
        //определяем, еда ли возле персонажа
        isFood = Physics2D.OverlapCircle(foodCheck.position, foodRadius, whatIsFood);


        //используем Input.GetAxis для оси Х. метод возвращает значение оси в пределах от -1 до 1.
        //при стандартных настройках проекта 
        //-1 возвращается при нажатии на клавиатуре стрелки влево (или клавиши А),
        //1 возвращается при нажатии на клавиатуре стрелки вправо (или клавиши D)
        move = Input.GetAxis("Horizontal");

        //в компоненте анимаций изменяем значение параметра Speed на значение оси Х.
        //приэтом нам нужен модуль значения
        anim.SetFloat("Speed", Mathf.Abs(move));

        //обращаемся к компоненту персонажа RigidBody2D. задаем ему скорость по оси Х, 
        //равную значению оси Х умноженное на значение макс. скорости
        myRigidbody.velocity = new Vector2(move * maxSpeed, myRigidbody.velocity.y);

        //если нажали клавишу для перемещения вправо, а персонаж направлен влево
        if (move > 0 && !isFacingRight)
            //отражаем персонажа вправо
            Flip();
        //обратная ситуация. отражаем персонажа влево
        else if (move < 0 && isFacingRight)
            Flip();
    }

    private void Update()
    {
        //если персонаж на земле и нажат пробел...
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            //устанавливаем в аниматоре переменную в false
            anim.SetBool("Ground", false);
            //прикладываем силу вверх, чтобы персонаж подпрыгнул
            myRigidbody.AddForce(new Vector2(0, 900));
        }

    }

    void OnCollisionEnter2D(Collision2D colis)
    {
        if (colis.gameObject.name == "Janitor (1)")
            ;// Destroy(gameObject);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var SemkaController = col.gameObject.GetComponent<SemkaController>();
        if (col.gameObject.tag == "Food" && SemkaController.onGround)
        {
            Destroy(col.gameObject);
        }
    }
    /// <summary>
    /// Метод для смены направления движения персонажа и его зеркального отражения
    /// </summary>
    private void Flip()
    {
        //меняем направление движения персонажа
        isFacingRight = !isFacingRight;
        //получаем размеры персонажа
        Vector3 theScale = transform.localScale;
        //зеркально отражаем персонажа по оси Х
        theScale.x *= -1;
        //задаем новый размер персонажа, равный старому, но зеркально отраженный
        transform.localScale = theScale;
    }
}