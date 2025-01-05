# delete_meta_files.py
# This script is used to delete all .meta files under the PixelWorld/Assets folder.

import os

def remove_meta_files(folder_path):
    """
    Recursively remove all .meta files under the given folder path.
    """
    # Walk through the folder structure
    for root, dirs, files in os.walk(folder_path):
        for file_name in files:
            # Check if the file is a .meta file
            if file_name.endswith('.meta'):
                file_path = os.path.join(root, file_name)
                try:
                    os.remove(file_path)
                    print(f"Removed: {file_path}")
                except OSError as e:
                    print(f"Error removing {file_path}: {e}")

if __name__ == "__main__":
    # By default, we'll look for the Assets folder inside the same directory
    # structure as the script folder (../../PixelWorld/Assets) if needed.
    # You can modify the path to suit your project structure.
    script_dir = os.path.dirname(__file__)
    assets_path = os.path.normpath(os.path.join(script_dir, '..', 'PixelWorld', 'Assets'))

    print("Starting to remove .meta files in:", assets_path)
    remove_meta_files(assets_path)
    print("Finished removing .meta files.")
