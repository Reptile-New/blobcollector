from PIL import Image, ImageDraw, ImageFont
import os
import colorsys

# Créer le dossier de sortie s'il n'existe pas
output_dir = "Assets/Sprites/Blobs"
os.makedirs(output_dir, exist_ok=True)

# Taille de l'image
size = 256

# Générer 50 blobs avec des couleurs variées
for i in range(1, 51):
    # Créer une couleur unique pour chaque blob (en utilisant HSV pour avoir des couleurs bien réparties)
    hue = (i - 1) / 50.0  # Répartir sur tout le spectre
    saturation = 0.7
    value = 0.9

    # Convertir HSV en RGB
    r, g, b = colorsys.hsv_to_rgb(hue, saturation, value)
    color = (int(r * 255), int(g * 255), int(b * 255))

    # Créer l'image
    img = Image.new('RGBA', (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    # Dessiner un cercle de couleur
    padding = 20
    draw.ellipse([padding, padding, size-padding, size-padding], fill=color)

    # Ajouter le numéro au centre
    try:
        font = ImageFont.truetype("arial.ttf", 60)
    except:
        font = ImageFont.load_default()

    text = str(i)
    # Calculer la position du texte pour le centrer
    bbox = draw.textbbox((0, 0), text, font=font)
    text_width = bbox[2] - bbox[0]
    text_height = bbox[3] - bbox[1]
    x = (size - text_width) // 2
    y = (size - text_height) // 2 - 10

    # Dessiner le texte en blanc avec un contour noir
    for offset_x, offset_y in [(-2,-2), (-2,2), (2,-2), (2,2)]:
        draw.text((x + offset_x, y + offset_y), text, fill=(0, 0, 0), font=font)
    draw.text((x, y), text, fill=(255, 255, 255), font=font)

    # Sauvegarder l'image
    filename = f"blob_{i:03d}.png"
    img.save(os.path.join(output_dir, filename))
    print(f"Créé: {filename}")

print(f"\n✓ {50} blobs placeholder créés dans {output_dir}")
