#!/bin/bash

curl -w @- -o /dev/null -s "$@" <<'EOF'
    
         time_total:  %{time_total}\n
EOF