# mcpack_Extractor.py
# This script is used for extracting .mcpack files by treating them as zip files.

import os
import zipfile

def extract_mcpack(mcpack_path, output_folder=None):
    """
    Extract a .mcpack file to the specified output folder.
    By default, it will extract to PixelWorld/Assets/Textures.
    """
    # Check if the .mcpack file exists
    if not os.path.isfile(mcpack_path):
        print("The .mcpack file does not exist.")
        return

    # If no output folder is specified, use the PixelWorld/Assets/Textures folder
    if not output_folder:
        # Build the default path to PixelWorld/Assets/Textures
        script_dir = os.path.dirname(__file__)
        default_assets_textures = os.path.join(script_dir, '..', 'PixelWorld', 'Assets', 'Textures')
        # Normalize path to avoid issues on different OS
        output_folder = os.path.normpath(default_assets_textures)

    # Create output folder if it doesn't exist
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)

    # Try to unzip the file
    try:
        with zipfile.ZipFile(mcpack_path, 'r') as zip_ref:
            zip_ref.extractall(output_folder)
            print(f"Extraction completed. Files are extracted to: {output_folder}")
    except zipfile.BadZipFile:
        print("Error: The .mcpack file is not a valid zip file or is corrupted.")


if __name__ == "__main__":
    # Example usage:
    # 1. Replace '../Cite_Assets/Textures_01.mcpack' with your .mcpack file path
    # 2. Optionally, replace the second argument with desired extraction path
    # 3. Run the script with: python mcpack_Extractor.py
    mcpack_file_path = '../Cite_Assets/Textures_01.mcpack'
    # By not specifying a second argument, it will extract to PixelWorld/Assets/Textures
    extract_mcpack(mcpack_file_path)
