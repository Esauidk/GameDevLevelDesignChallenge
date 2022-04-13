extends Spatial

var deaths = 0

func _on_Player_player_died():
	$Player.translation = Vector3(0, 0.5, 0)
	$Player.velocity = Vector3.ZERO
	deaths += 1
	$Slap.play()
