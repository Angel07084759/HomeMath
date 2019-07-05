git add .
#git commit -m "`date '+%Y-%m-%d %H:%M:%S'`"
read -p "Enter message: " userInput
git commit -m "`date '+%Y-%m-%d %H:%M:%S'` => $userInput"
git push
read -p "Press [Enter] to continue..."