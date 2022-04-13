extends Spatial

var deaths = 0

func _physics_process(delta):
	$Player/CameraBase.rotation_degrees.y = -$Player.velocity.x * 2
	$Player/CameraBase.rotation_degrees.x = -$Player.velocity.z * 2
#	$InfoLabel.text = str($Player/CoyoteTime.time_left)

func _on_Player_player_died():
	$Player.translation = Vector3(0, 0.5, 0)
	$Player.velocity = Vector3.ZERO
	deaths += 1
	$Slap.play()
