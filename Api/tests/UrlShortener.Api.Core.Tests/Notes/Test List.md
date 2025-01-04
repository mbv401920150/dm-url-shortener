# TEST LIST
- Unique tokens
- Accept multiple Ranges
- _tokenRange on TokenProvider can be null when getting the token   

# DONE
- Range as a type
- Check if the end of range is gt start
- Should the token range use longs instead of ints?   
    **R/ The long should be the best way to handle this.**    
    `Int Max value  =   2,147,483,647`  
    `Long Max value =   9,223,372,036,854,775,807`  
    `62 power to 7  =   3,521,614,606,208 (Possible combinations)`
- Return the short url from a new url.
- When adding a new url, should save it.
- Who's the creator 
- When was created.