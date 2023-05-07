#import pyrebase
import firebase_admin
from firebase_admin import storage, db
import json
import threading

#config = json.load(open("firebaseconfig.json"))

cred_obj = firebase_admin.credentials.Certificate(rf'firebaseconfig.json')
default_app = firebase_admin.initialize_app(cred_obj, {'databaseURL':rf"https://terraria-block-texture-puller-default-rtdb.firebaseio.com",
                                                       'storageBucket':rf"terraria-block-texture-puller.appspot.com"})
#firebase = pyrebase.initialize_app(config)
#storage = firebase.storage()
#database = firebase.database()

ref = db.reference("/")

tile_names = []

# Gets all the
for item in ref.get():
    item = str(item).strip()
    tile_names.append(item)
    #print(item)

storage_bucket = storage.bucket()
blobs = list(storage_bucket.list_blobs())#None, None, rf"Tiles/Tiles_"))

threads = []

for blob in blobs:
    #cleans up the file path for comparison
    attribute_list = str(blob).split(",")
    attribute_list[1] = attribute_list[1].strip()
    attribute_list[1] = attribute_list[1].strip(".png")
    attribute_list[1] = attribute_list[1].removeprefix("Tiles/")

    image_name = attribute_list[1]

    if image_name not in tile_names:
        print(f"Deleting {image_name}...")
        t = threading.Thread(target=blob.delete)
        t.start()
        threads.append(t)
    else:
        print(f"Keeping {image_name}...")

for thread in threads:
    thread.join()
    print("Image deleted!")
