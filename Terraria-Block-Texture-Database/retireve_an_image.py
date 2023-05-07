import firebase_admin
from firebase_admin import storage, db
import numpy as np
import io
from PIL import Image

#initializes the app and storage to grab from
cred_obj = firebase_admin.credentials.Certificate(rf'firebaseconfig.json')
default_app = firebase_admin.initialize_app(cred_obj, {'databaseURL':rf"https://terraria-block-texture-puller-default-rtdb.firebaseio.com",
                                                       'storageBucket':rf"terraria-block-texture-puller.appspot.com"})

ref = db.reference("/")

file_name = rf"Tiles_22" #change file name here to retrieve a different texture file. Try Tiles_1, Tiles_22, or Tiles_25

json_object = ref.child(file_name)

image_to_pull = json_object.get()["content"]["export"] # grabs the file name to get from storage

# print(image_to_pull)

storage_bucket = storage.bucket()
image_blob = storage_bucket.blob(rf"Tiles/{image_to_pull}") # Blob
byte_array = np.frombuffer(image_blob.download_as_string(), np.uint8) # Blob to Byte Array
image = Image.open(io.BytesIO(byte_array)) # Byte Array to Image

image.show() #Should show a bunch of dirt block textures