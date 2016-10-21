Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment 0.1 0.1 1.1 -e LearningRate -s Unipolar" -RedirectStandardOutput "lr_0,1-1,1_u.csv"
Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment 0.1 0.1 1.1 -e LearningRate -s Bipolar" -RedirectStandardOutput "lr_0,1-1,1_b.csv"

Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment 0.01 0.01 0.1 -e LearningRate -a -t 0.4" -RedirectStandardOutput "lr_a_0,01-0,1.csv"
Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment 0.1 0.02 0.3 -e LearningRate -a -t 0.4" -RedirectStandardOutput "lr_a_0,1-0,3.csv"
Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment 0.1 0.1 1 -e LearningRate -a -t 0.4" -RedirectStandardOutput "lr_a_0,1-1,1.csv"
 
Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment 0.1 0.1 1.1 -e LearningRate" -RedirectStandardOutput "lr_a_0-1,1.csv"
Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment 1 1 10 -e LearningRate" -RedirectStandardOutput lr_1-10.csv

Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment 0 0.1 1 -e AdalineThreshold" -RedirectStandardOutput at_0-1.csv
Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment 0.3 0.01 0.4 -e AdalineThreshold" -RedirectStandardOutput "at_0,3-0,4.csv"

Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment 0 0.1 1 -e InitialWeights" -RedirectStandardOutput iw_0-1.csv
Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment 1 1 11 -e InitialWeights" -RedirectStandardOutput iw_1-11.csv

Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment 0 0.1 1 -e InitialWeights -a -t 0.4" -RedirectStandardOutput iw_a_0-1.csv
Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment 1 1 11 -e InitialWeights -a -t 0.4" -RedirectStandardOutput iw_a_1-11.csv

Start-Process -NoNewWindow -FilePath perceptron.exe -ArgumentList "experiment -e AdalineError -t 0.33" -RedirectStandardOutput "ae_0,33.csv"