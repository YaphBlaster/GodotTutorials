extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

const SAVE_DATA_PATH = "res://save_data.json"

var default_save_data = {
	highscore = 0
}


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func save_data_to_file(save_data):
	# get the save data dictionary as a string
	var json_string = to_json(save_data)
	
	# create a save file
	var save_file = File.new()

	# open the file and write (save)
	save_file.open(SAVE_DATA_PATH, File.WRITE)

	# save the json string to the file
	save_file.store_line(json_string);
	save_file.close()

func load_data_from_file():
	# create a new file
	var save_file = File.new()

	# check if the file exists at a given path
	if not save_file.file_exists(SAVE_DATA_PATH):
		return default_save_data
	
	# open the file and allow read access
	save_file.open(SAVE_DATA_PATH, File.READ)

	# parse_json converts json to a dictionary
	var save_data = parse_json(save_file.get_as_text())
	save_file.close()
	return save_data

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
