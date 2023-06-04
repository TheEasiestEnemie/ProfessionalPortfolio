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

tile_file_ids = [
    "0", "1", "2", "2_Beach", "6", "7", "8", "9", "22", "23", "25", "30", "37", "38",
    "39", "40", "41", "43", "44", "45", "46", "47", "48", "51", "53", "54", "56", "57",
    "58", "59", "60", "63", "64", "65", "66", "67", "68", "70", "75", "76", "107", "108",
    "109", "111", "112", "116", "117", "118", "119", "120", "121", "122", "123",
    "124", "130", "131", "140", "145", "146", "147", "148", "150", "151", "152",
    "153", "154", "155", "156", "157", "158", "159", "161", "162", "163", "164",
    "166", "167", "168", "169", "170", "175", "176", "177", "179", "180", "181",
    "182", "183", "188", "189", "190", "191", "192", "193", "194", "195", "196", 
    "197", "198", "199", "200", "202", "203", "204", "206", "208", "211", "221",
    "222", "223", "224", "225", "226", "229", "230", "232", "234", "235", "248",
    "249", "250", "251", "252", "253", "255", "256", "257", "258", "259", "260",
    "261", "262", "263", "264", "265", "266", "267", "268", "273", "274", "284",
    "311", "312", "313", "321", "322", "325", "326", "327", "328", "329", "336",
    "340", "341", "342", "343", "344", "345", "346", "347", "348", "350", "351",
    "357", "367", "368", "369", "370", "371", "381", "383", "384", "385", "396",
    "397", "398", "399", "400", "401", "402", "403", "404", "407", "408", "409",
    "415", "416", "417", "418", "426", "430", "431", "432", "433", "434", "446",
    "447", "448", "458", "459", "460", "472", "473", "474", "477", "478", "479",
    "492", "496", "498", "500", "501", "502", "503", "507", "508", "512", "513",
    "514", "515", "516", "517", "534", "535", "536", "537", "539", "540", "541",
    "546", "561", "562", "563", "566", "574", "575", "576", "577", "578", "618",
    "625", "626", "627", "628", "633", "634", "635", "641", "659", "661", "662",
    "666"
]

storage_bucket = storage.bucket()
avg_colors = []

for id in tile_file_ids:
    id = "Tiles_" + id


    json_object = ref.child(id)

    image_to_pull = json_object.get()["content"]["export"] # grabs the file name to get from storage

    print(image_to_pull)

    image_blob = storage_bucket.blob(rf"Tiles/{image_to_pull}") # Blob
    byte_array = np.frombuffer(image_blob.download_as_string(), np.uint8) # Blob to Byte Array
    
    print("Retrieving ...")
    
    image = Image.open(io.BytesIO(byte_array)) # Byte Array to Image

    pixel_values = list(image.getdata())
    total_red = 0
    total_green = 0
    total_blue = 0
    total_alpha = 0
    pixel_count = 0
    for pixel in pixel_values:
        pixel_count += 1
        total_red += pixel[0]
        total_green += pixel[1]
        total_blue += pixel[2]
        total_alpha += pixel[3]

    avg_red = total_red / pixel_count
    avg_green = total_green / pixel_count
    avg_blue = total_blue / pixel_count
    avg_alpha = total_alpha / pixel_count

    avg_color = [avg_red, avg_green, avg_blue, avg_alpha]
    avg_colors.append(avg_color)

print(avg_colors)

# image.show() #Should show a bunch of block textures