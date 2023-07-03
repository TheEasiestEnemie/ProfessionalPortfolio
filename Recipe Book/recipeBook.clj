(use 'clojure.string)
(require '[clojure.string :as str])


(defn read [file-name]
      (with-open [rdr (clojure.java.io/reader file-name)]
                 (reduce conj [] (line-seq rdr)))
      )

(defn write
      ([file-name] ;; !!!!!!!-USED TO CLEAR THE GIVEN FILE. USE WITH CAUTION!-!!!!!!!
      (with-open [w (clojure.java.io/writer file-name :append false)]
           (.write w (str ""))   ;; erases the file
           )
      )

      ([file-name recipe]
      (with-open [w (clojure.java.io/writer file-name :append true)]
           (.write w (str recipe "\n"))   ;; intended for recipes that can be made with available ingredients
            )
      )

      ([file-name ingredient amount]
      (with-open [w (clojure.java.io/writer file-name :append true)]
           (.write w (str ingredient "-" amount "\n"))   ;; intended for key pairs ("tomato"-2.0)
            )
      )
      )

(def recipes (read "recipes.txt"))

(defn add-ingredient
      ([]
      (println "Enter your ingredient:")
      (let [ingredient (read-line)
            list (read "ingredients.txt")]
           ;;(println list)
           (if (not (empty? (rest list)))
              (if (not= (trim (first list)) (trim ingredient))
                (do (add-ingredient ingredient (rest list))
                (println "Ingredient found!"))
                  (do (add-ingredient ingredient list 0))
               )
             )
           )
       )

      ([ingredient list]
       (if (not (empty? (rest list)))
          (if (not= (trim (first list)) (trim ingredient))
            (do (add-ingredient ingredient (rest list)))
           (do (println "Ingredient found!")
            (add-ingredient ingredient list 0))
           )
         (println "Ingredient not found in list.")
         )
       )

      ([ingredient list amount]
       (println "Enter the amount:")
       (let [real-amount (Float/parseFloat (read-line))]
            (write "yourIngredients.txt" ingredient real-amount)
            )
        )
      )

(defn custom-split [line index]
      (let [key-pair (str/split line #"-")]
            (if (= index 0)
                  (get key-pair 0)
                  (if (= index 1)
                        (get key-pair 1)
                  )
            )
      )
)

(defn split-and-compare 
      ([line ingredient-list]
            ;;(println line (first ingredient-list))
            ;;(println line (first (rest ingredient-list)))

            (let [ingredient1 (custom-split line 0)
                  amount1 (Float/parseFloat (trim (custom-split line 1)))
                  ingredient2 (custom-split (first ingredient-list) 0)
                  amount2 (Float/parseFloat (trim (custom-split (first ingredient-list) 1)))
                  ]

                  ;;(println ingredient2 amount2)
                  (if (= (trim ingredient1) (trim ingredient2))
                        (if (< amount1 amount2)
                              true
                              (println "Don't have enough of " ingredient1)
                        )
                        (if (not (empty? (rest ingredient-list)))
                              (split-and-compare line (first (rest ingredient-list)) (rest ingredient-list))
                              false
                        )
                  )
            )
      )

      ([line1 line2 your-ingredients]
            ;;(println line1 line2)
            (if (= (trim (first your-ingredients)) "*")
            (Float/parseFloat (trim (custom-split line1 1)))
            (let [ingredient1 (custom-split line1 0)
                  amount1 (Float/parseFloat (trim (custom-split line1 1)))
                  ingredient2 (custom-split line2 0)
                  amount2 (Float/parseFloat (trim (custom-split line2 1)))
                  ]

                  ;;(println ingredient2 amount2)
                  (if (= (trim ingredient1) (trim ingredient2))
                        (if (< amount1 amount2)
                              true
                              (println "Don't have enough of " ingredient1)
                        )
                        (if (not (empty? (rest your-ingredients)))
                              (split-and-compare line1 (first (rest your-ingredients)) (rest your-ingredients))
                              false
                        )
                  )
            )
            )
      )
)



(defn completes-recipe [list1 list2]
      (if (= (trim (first list2)) "*")
      false
      (let [ingredient1 (custom-split (first list1) 0)
            ingredient2 (custom-split (first list2) 0)
            ]
            ;;(println ingredient1 ingredient2)
            (if (= (trim ingredient1) (trim ingredient2))
                  (do 
                        ;;(println (str "list1: " (empty? (rest list1))))
                        ;;(println list1)
                        ;;(println (str "list2: " (empty? (rest list2))))
                        ;;(println list2)
                        (if (and (or (empty? list2) (empty? list1)) (not (and (empty? list2) (empty? list1))))
                              false
                              (if (and (empty? (rest list1)) (= (trim (first (rest list2))) "*"))
                                    (do ;;(println "Found completed recipe")
                                    true)
                                    (if (empty? (rest list1))
                                          false
                                          (do ;;(println "Haven't found a completed recipe yet.")
                                          (completes-recipe (rest list1) (rest list2)))
                                    )
                              )
                        )
                  )
                  false
            )
      )
      )
)

(defn cleanup []
      (write "completedRecipes.txt")
)

(defn view-txt-file [filename]
      (let [igdnts (read filename)]
           (run! println igdnts)
           )
      (println)
      (println "Press enter to continue:")
      (read-line)
      )

(defn find-recipes 
      ([]
            (find-recipes recipes)
      )

      ([recipe-list]
            (if (= (trim (first recipe-list)) "*")
                  (if (not (empty? (rest recipe-list)))
                        (recur (rest recipe-list))
                        (view-txt-file "completedRecipes.txt")
                  )
                  (let [current-recipe (first recipe-list)]
                        ;;(println recipe-list)
                        ;;(println current-recipe)
                        (if (not (empty? (rest recipe-list)))
                              (find-recipes current-recipe [] (rest recipe-list) (rest recipe-list))
                              (view-txt-file "completedRecipes.txt")
                        )
                  )
            )
      )

      ([current-recipe ingredients-left recipe-list-acc current-recipe-list]
            (if (= (trim (first recipe-list-acc)) "*")
                  (if (not (empty? (rest recipe-list-acc)))
                        (find-recipes (rest recipe-list-acc))
                        (view-txt-file "completedRecipes.txt")
                  )
                  (do 
                  ;;(println (rest recipe-list-acc))
                  (let [current-ingredient (first recipe-list-acc)
                        amount-required (split-and-compare current-ingredient (read "yourIngredients.txt"))
                        ingredients-left-acc (do (if (= amount-required true)
                                                      (conj ingredients-left current-ingredient)
                                                      (conj ingredients-left (str "INVALID-0.0"))
                                                ))
                        ]
                        ;;(println current-ingredient)
                        ;;(println ingredients-left-acc recipe-list-acc)
                        ;;(println (completes-recipe ingredients-left-acc current-recipe-list))
                        (if (not (empty? (rest recipe-list-acc)))
                              (if (completes-recipe ingredients-left-acc current-recipe-list)
                                    (do 
                                          (write "completedRecipes.txt" current-recipe)
                                          (find-recipes (rest recipe-list-acc))
                                    )
                                    (recur current-recipe ingredients-left-acc (rest recipe-list-acc) current-recipe-list)
                              )     
                              (view-txt-file "completedRecipes.txt")
                        )
                        )
                  )
            )
      )

)


(defn clear-ingredients []
      (write "yourIngredients.txt")
)



(defn main []
      (loop []
            (println "========================")
            (println "    RECIPE BOOK MENU    ")
            (println "========================")
            (println "")
            (println "1) Add an ingredient")
            (println "2) View possible recipes")
            (println "3) Clear ingredients")
            (println "4) View current ingredients")
            (println "5) Quit")
            (println "")
            (println "Enter an option 1-5: ")
            (let [choice (Integer/parseInt (read-line))]
                 (println)
                 (cond (= choice 1)
                          (add-ingredient)
                       (= choice 2)
                          (do (cleanup) (find-recipes))
                       (= choice 3)
                          (clear-ingredients)
                       (= choice 4)
                          (view-txt-file "yourIngredients.txt")
                       )
                 (if (not= choice 5)
                   (recur)
                   (println "Goodbye!")
                   )
                 )
        )
      )
(main)