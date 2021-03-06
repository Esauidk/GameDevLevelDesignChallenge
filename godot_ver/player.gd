extends KinematicBody

signal player_died

var velocity = Vector3.ZERO
export var gravity = Vector3.DOWN * 9.8 * 2

var jumping = false

func _physics_process(delta):
	var control_movement = Vector2(
		Input.get_axis("move_left", "move_right"),
		Input.get_axis("move_backward", "move_forward")
	)
	
	velocity += gravity * delta
	var right = Vector3(1, 0, 0).normalized()
	var forward = Vector3(0, 0, -1).normalized()
	velocity += right * control_movement.x * 2
	velocity += forward * control_movement.y * 2
	velocity.x *= 0.6
	velocity.z *= 0.6
	velocity = move_and_slide(velocity, Vector3.UP)
	
	
	if is_on_floor():
		$CoyoteTime.start()
	
	if $CoyoteTime.time_left > 0 and Input.is_action_just_pressed("jump"):
		velocity.y = 5.0
		jumping = true
	
	if Input.is_action_pressed("jump") and jumping and velocity.y > 0:
		velocity.y -= gravity.y * 0.8 * delta * min(velocity.y * 0.2, 1)
	else:
		jumping = false
	
	if translation.y < -4:
		emit_signal("player_died")


func _on_EnemyDetector_body_entered(body):
	emit_signal("player_died")

