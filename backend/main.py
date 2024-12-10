import os

def print_file_tree(root_path, indent=''):
    """
    Recursively print the file and directory structure of a given path.

    Args:
        root_path (str): The root directory path to start the tree from
        indent (str): Indentation string for hierarchical display
    """
    try:
        # Get the list of entries in the directory
        entries = os.listdir(root_path)

        # Sort entries to ensure consistent output
        entries.sort()

        for i, entry in enumerate(entries):
            full_path = os.path.join(root_path, entry)

            # Determine if this is the last entry to adjust tree visualization
            is_last = i == len(entries) - 1

            # Create appropriate prefix for tree visualization
            prefix = '└── ' if is_last else '├── '

            # Print the current entry
            print(f"{indent}{prefix}{entry}")

            # If it's a directory, recursively print its contents
            if os.path.isdir(full_path):
                # Adjust indentation for subdirectories
                new_indent = indent + ('    ' if is_last else '│   ')
                print_file_tree(full_path, new_indent)

    except PermissionError:
        print(f"{indent}└── [Permission Denied]")
    except Exception as e:
        print(f"{indent}└── [Error: {str(e)}]")

# Example usage
if __name__ == "__main__":
    # Replace this path with the directory you want to explore
    root_directory = "RecipeMeal/"
    print(root_directory)
    print_file_tree(root_directory)
