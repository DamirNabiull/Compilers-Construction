(func map (f n)
    (cond (isnull (head n))
        (return n)
        (return (cons 
                    (f (head n)) 
                    (map f (tail n))))))

(func mino (n) (return (minus n 1)))

(setq x (map mino (1 2 3 4 5)))
(head x)
(head (tail x))
