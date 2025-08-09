#!/bin/bash

# Zombie Defense Game - Butler Deploy Script
# Usage: ./deploy.sh [username/project-name] [version]

set -e  # Exit on any error

# Configuration
GODOT_EXECUTABLE="/Applications/Godot_mono.app/Contents/MacOS/Godot"
PROJECT_DIR=$(pwd)
EXPORTS_DIR="$PROJECT_DIR/Exports"

# Parameters
if [ $# -eq 0 ]; then
    echo "Usage: $0 <username/project-name> [version]"
    echo "Example: $0 jwjang/zombie-defense v1.0.0"
    exit 1
fi

ITCH_PROJECT=$1
VERSION=${2:-$(date +"%Y%m%d-%H%M%S")}

echo "🎮 Starting deployment for $ITCH_PROJECT"
echo "📦 Version: $VERSION"
echo "----------------------------------------"

# Clean and prepare export directories
echo "🧹 Cleaning export directories..."
rm -rf "$EXPORTS_DIR/Windows" "$EXPORTS_DIR/macOS"
mkdir -p "$EXPORTS_DIR/Windows" "$EXPORTS_DIR/macOS"

# Build solution first
echo "🔨 Building C# solution..."
"$GODOT_EXECUTABLE" --headless --build-solutions --quit || {
    echo "❌ Failed to build C# solution"
    exit 1
}

# Export platforms  
echo "🪟 Exporting Windows build..."
"$GODOT_EXECUTABLE" --headless --export-release "Windows Desktop" "$EXPORTS_DIR/Windows/ZombieDefense.exe" --quit || {
    echo "❌ Windows export failed"
    exit 1
}

echo "🍎 Exporting macOS build..."
"$GODOT_EXECUTABLE" --headless --export-release "macOS" "$EXPORTS_DIR/macOS/ZombieDefense.app" --quit || {
    echo "❌ macOS export failed"
    exit 1
}

# Verify exports
echo "✅ Verifying exports..."
for platform in "Windows/ZombieDefense.exe" "macOS/ZombieDefense.app"; do
    if [ ! -e "$EXPORTS_DIR/$platform" ]; then
        echo "❌ Export verification failed: $platform not found"
        exit 1
    fi
done

echo "📤 Uploading to itch.io..."

# Upload to itch.io using butler
butler push "$EXPORTS_DIR/Windows" "$ITCH_PROJECT:windows" --userversion "$VERSION" || {
    echo "❌ Failed to upload Windows build"
    exit 1
}

butler push "$EXPORTS_DIR/macOS" "$ITCH_PROJECT:mac" --userversion "$VERSION" || {
    echo "❌ Failed to upload macOS build"
    exit 1
}

echo "🎉 Deployment completed successfully!"
echo "📊 Build info:"
echo "  - Project: $ITCH_PROJECT"
echo "  - Version: $VERSION"
echo "  - Platforms: Windows, macOS"
echo "  - Upload time: $(date)"
echo ""
echo "🔗 Your game is now available at:"
echo "   https://$ITCH_PROJECT"