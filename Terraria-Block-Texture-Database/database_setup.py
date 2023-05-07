import firebase_admin
import json
from firebase_admin import db

cred_obj = firebase_admin.credentials.Certificate(rf'C:\Users\13857\OneDrive\Desktop\Professional Portfolio\Terraria-Block-Texture-Database\firebaseconfig.json')
default_app = firebase_admin.initialize_app(cred_obj, {'databaseURL':rf"https://terraria-block-texture-puller-default-rtdb.firebaseio.com"})
ref = db.reference("/")

with open("json_tiles.json", "r") as f:
	file_contents = json.load(f)
ref.set(file_contents)