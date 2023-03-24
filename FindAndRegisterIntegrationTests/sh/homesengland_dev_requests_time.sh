seq 1 499 \
| xargs -P50 ./curltime.sh "https://ahofr-nonprod-dev.homesengland.org.uk/find-organisations-selling-shared-ownership-homes/organisations-that-sell-shared-ownership-homes" \
-X POST -d "Area=Adur" \
| awk '{print NR "," $2*1000}' \
> results.csv 
#awk '{s+=$1} END {print s}'