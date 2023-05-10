(setq a 0)

(while true (cond (lesseq a 10) (setq a (plus 1 a)) (break)))

(print a)