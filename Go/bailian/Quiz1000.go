package bailian

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

type Quiz1000 struct {
}

func (quiz *Quiz1000) GetIntroduce() string {
	return `
	描述
	Calculate a + b

	输入
	Two integer a,,b (0 ≤ a,b ≤ 10)
	输出
	Output a + b
	样例输入
	1 2
	样例输出
	3`
}

func (quiz *Quiz1000) Test() {
	var input string
	scanner := bufio.NewScanner(os.Stdin)
	if scanner.Scan() {
		input = scanner.Text()
	}
	inputs := strings.Split(input, " ")
	a, _ := strconv.Atoi(inputs[0])
	b, _ := strconv.Atoi(inputs[1])
	fmt.Println(a + b)
	return
}
