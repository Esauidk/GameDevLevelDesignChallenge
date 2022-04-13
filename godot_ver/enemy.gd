extends KinematicBody

export(Array, NodePath) var waypoints
export var speed = 4.0

var waypoint_locations = []
var waypoint_index = 0

func _ready():
	for waypoint in waypoints:
		waypoint_locations.append(get_node(waypoint).get_global_transform().origin)
	if waypoint_locations.size() == 0:
		waypoint_locations.append(get_global_transform().origin)


func _physics_process(delta):
	var target = waypoint_locations[waypoint_index]
	
	var displacement = get_global_transform().origin - target
	
	translate(-displacement.normalized() * delta * speed)
	
	if displacement.length() < delta * speed * 2:
		waypoint_index += 1
	
	if waypoint_index >= waypoint_locations.size():
		waypoint_index = 0



#func _on_Enemy_body_entered(body):
#	print("foo")
#	get_tree().call_group("player", "enemy_collided", self)
