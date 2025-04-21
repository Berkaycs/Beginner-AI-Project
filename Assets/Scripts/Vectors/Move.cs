using UnityEngine;

public class Move : MonoBehaviour
{
    #region Vector Info
    // Vector shows the direction and its length is called as magnitude. Vectors length can be found with Pythogoras theorem.
    // For instance, Vector (3,4) length is (root of 3^2 + 4^2) = 5.
    // (3,4) point and (3,4) vector is different. There is only one (3,4) point but (3,4) vector could be anywhere in space. 
    #endregion

    #region References
    [SerializeField] private GameObject _target;
    [SerializeField] private float _speed;
    private Vector3 _direction;
    #endregion

    private void Start()
    {
        #region PART 1
        // in this case the characters move by Vector(-6,0,8)
        // if their current position is (1,0,2), after moving their new position will be (-5,0,10)
        //transform.Translate(_target.transform.position);
        #endregion

        #region PART 2
        // Calculating the distance between two objects
        //_direction = _target.transform.position - transform.position;
        // It will add the distance to the object, so they'll be in the same position
        //transform.position += _direction; 
        // You can also use this to move the object
        //transform.Translate(_direction);
        #endregion
    }

    private void LateUpdate()
    {
        #region PART 2
        // We want to calculate the movement at the end (because of animations etc.)
        // We make the direction length 1 with normalized. So all characters length of step are equal now.
        //Vector3 velocity = _direction.normalized * _speed * Time.deltaTime;
        //transform.position += velocity;

        // If we don't normalized the direction, each character has different length of step based on the direction length.
        // However the characters will arrive the related point at the same time.
        //transform.position += _direction * 0.01f * Time.deltaTime;
        #endregion

        #region PART 3
        // Calculating Vector each frame
        _direction = _target.transform.position - transform.position;

        // Rotating object based on the target's position
        transform.LookAt(_target.transform.position);

        // Until the distance bigger than 2, objects will move to the target.
        if (_direction.magnitude > 2)
        {
            Vector3 velocity = _direction.normalized * _speed * Time.deltaTime;
            transform.position += velocity;
        }
        #endregion
    }
}
