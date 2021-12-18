package main

import (
	"bufio"
	"fmt"
	"os"
	"strings"

	"github.com/NoGTNoHappy/OpenJudgeCN/Go/bailian"
)

const (
	guidance       = "Please input test's ID. -l or --list to get available tests. Input \"Q\" to exist."
	testNotFound   = "%s is not found in avaliable tests. Please try to get available tests.\r\n"
	runTestConfirm = "Input \"R\" to run, \"Q\" to return, or \"H\" to get help."
	continueAsk    = "Continue? Y/N"
	bye            = "Bye!"
)

var (
	quizMap = map[string]bailian.Quiz{}
)

func main() {
	cancel := false
mainLoop:
	for !cancel {
		defer func() {
			err := recover()
			if err != nil {
				fmt.Println(err)
				if continueJudge() {
					main()
				}
			}
		}()
		fmt.Println(guidance)

		for {
			var cmd string
			scanner := bufio.NewScanner(os.Stdin)
			if scanner.Scan() {
				cmd = scanner.Text()
			}

			switch cmd {
			case "Q", "q":
				break mainLoop
			case "-l", "--list", "-I", "-1":
				fmt.Println(findAllTests())
			default:
				runTest(cmd)
			}

			fmt.Println()
			fmt.Println(guidance)
		}
	}
	fmt.Println(bye)
	fmt.Scanln()
}

func findAllTests() (res string) {
	res = ""
	files, _ := os.ReadDir("./bailian/")
	for _, file := range files {
		fn := file.Name()
		if strings.HasPrefix(fn, "Quiz") {
			res = res + fn[0:len(fn)-3] + ", "
		}
	}

	res = strings.TrimSuffix(res, ", ")
	return
}

func continueJudge() (res bool) {
	fmt.Println(continueAsk)
	var isContinue string
	fmt.Scanln(&isContinue)
	res = !(isContinue == "Y" || isContinue == "y")
	return
}

func runTest(cmd string) {
	target, isExist := quizMap[cmd]
	if !isExist {
		fmt.Printf(testNotFound, cmd)
		return
	}

	fmt.Println(runTestConfirm)
	cancel := false
	for !cancel {
		var cmd string
		scanner := bufio.NewScanner(os.Stdin)
		if scanner.Scan() {
			cmd = scanner.Text()
		}
		switch cmd {
		case "R", "r":
			target.Test()
			cancel = true
		case "H", "h":
			fmt.Println(target.GetIntroduce())
			fmt.Println()
			fmt.Println(runTestConfirm)
		case "Q", "q":
			cancel = true
		default:
			fmt.Println(runTestConfirm)
		}
	}
}

func init() {
	quizMap = make(map[string]bailian.Quiz)
	quizMap["Quiz1000"] = &bailian.Quiz1000{}
}
