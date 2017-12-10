set term png
set output prd.png

plot \
    "graph/naive-64-64.graph"   with linespoints lt rgb "#8B0000" title "64-64 naive", \
    "graph/smart-64-64.graph"   with linespoints lt rgb "#CD5C5C" title "64-64 oblivious", \
    "graph/naive-64-1024.graph" with linespoints lt rgb "#1E90FF" title "64-1024 naive", \
    "graph/smart-64-1024.graph" with linespoints lt rgb "#00CED1" title "64-1024 oblivious", \
    "graph/naive-64-4096.graph" with linespoints lt rgb "#DAA520" title "64-4096 naive", \
    "graph/smart-64-4096.graph" with linespoints lt rgb "#FFD700" title "64-4096 oblivious", \
    "graph/naive-512-512.graph" with linespoints lt rgb "#008000" title "512-512 naive", \
    "graph/smart-512-512.graph" with linespoints lt rgb "#8FBC8F" title "512-512 oblivious", \
    "graph/naive-4096-64.graph" with linespoints lt rgb "#D2691E" title "4096-64 naive", \
    "graph/smart-4096-64.graph" with linespoints lt rgb "#FF7F50" title "4096-64 oblivious"