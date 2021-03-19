from PIL import Image
import sys

if len(sys.argv) == 2:
    image = Image.open(sys.argv[1])
    image.save(sys.argv[1])