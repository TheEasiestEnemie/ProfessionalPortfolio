# This file will grab all the files in the terraria content folder that are tiles
# select the parts of the file that are texture images, convert them to json,
# and write them to the json_tiles.json file.

import json

# IMPORTANT! Make sure to create a BRAND new json file called json_tiles.json in this directory
# every time you run this file or this won't work and just duplicate data.

# A list of all the Tile numbers we want. All the others aren't placeable blocks.
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

raw_data = []

# The path to all the json files we need on my personal laptop. If you want to 
# run this on your computer, make sure you have all the .xnb files unpacked and
# in a single directory and change the file path accordingly.
# 
# I used xnbcli to unpack my own terraria files, but you can use any unpacking 
# software for .xnb. You'll also need to find the xnb files in the game folders
# of Terraria.
path = rf"C:\Users\13857\OneDrive\Desktop\CSE 251\ProfessionalPortfolio\Terraria-Block-Texture-Database\xnbcli\xnbcli\unpacked"


# Takes the tile ids from the list above and grabs all the json data from the 
# directory.
for id in tile_file_ids:
    with open(rf"{path}\Tiles_{id}.json", "r") as file:

        data = file.read()
        raw_data.append(data)
        json_data = json.dumps(data)
        

# Writes the new json data to a file to be read by other files later.

with open("json_tiles.json", "w") as json_file:
    json_file.write("{")
    for i in range(len(raw_data)):
        json_file.write(f"\"Tiles_{tile_file_ids[i]}\": ")
        json_file.write(raw_data[i])
        json_file.write(", ")
        # Make sure to remove the final comma in the json file referrenced
    json_file.write("}")