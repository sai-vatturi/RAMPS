-- ================================================
-- Comprehensive Sample Data Population Script
-- ================================================

-- ================================================
-- Insert Users
-- ================================================
INSERT INTO Users (
    Username, 
    FirstName, 
    LastName, 
    Email, 
    PasswordHash, 
    PhoneNumber, 
    Role, 
    IsApproved, 
    IsActive, 
    IsEmailVerified, 
    EmailVerificationToken, 
    PasswordResetToken, 
    PasswordResetTokenExpiry
)
VALUES 
    -- Meal Planners
    ('mealplanner1', 'Meal', 'PlannerOne', 'mealplanner1@example.com', 
     '$2a$11$gSxpmTrwPSvT1zFcfJT7Z.5HaVSZ44q9DWu8JBeYh92oGzCUxyl7m', 
     '1234567891', 'MealPlanner', 1, 1, 1, 'token_mp1', NULL, NULL),
     
    ('mealplanner2', 'Meal', 'PlannerTwo', 'mealplanner2@example.com', 
     '$2a$11$gSxpmTrwPSvT1zFcfJT7Z.5HaVSZ44q9DWu8JBeYh92oGzCUxyl7m', 
     '1234567892', 'MealPlanner', 1, 1, 1, 'token_mp2', NULL, NULL),
     
    -- Chefs
    ('chef1', 'Chef', 'One', 'chef1@example.com', 
     '$2a$11$gSxpmTrwPSvT1zFcfJT7Z.5HaVSZ44q9DWu8JBeYh92oGzCUxyl7m', 
     '1234567893', 'Chef', 1, 1, 1, 'token_chef1', NULL, NULL),
     
    ('chef2', 'Chef', 'Two', 'chef2@example.com', 
     '$2a$11$gSxpmTrwPSvT1zFcfJT7Z.5HaVSZ44q9DWu8JBeYh92oGzCUxyl7m', 
     '1234567894', 'Chef', 1, 1, 1, 'token_chef2', NULL, NULL),
     
    -- Nutritionists
    ('nutritionist1', 'Nutrition', 'IstOne', 'nutritionist1@example.com', 
     '$2a$11$gSxpmTrwPSvT1zFcfJT7Z.5HaVSZ44q9DWu8JBeYh92oGzCUxyl7m', 
     '1234567895', 'Nutritionist', 1, 1, 1, 'token_nut1', NULL, NULL),
     
    ('nutritionist2', 'Nutrition', 'IstTwo', 'nutritionist2@example.com', 
     '$2a$11$gSxpmTrwPSvT1zFcfJT7Z.5HaVSZ44q9DWu8JBeYh92oGzCUxyl7m', 
     '1234567896', 'Nutritionist', 1, 1, 1, 'token_nut2', NULL, NULL),
     
    -- Regular Users
    ('user1', 'Regular', 'UserOne', 'user1@example.com', 
     '$2a$11$gSxpmTrwPSvT1zFcfJT7Z.5HaVSZ44q9DWu8JBeYh92oGzCUxyl7m', 
     '1234567897', 'User', 1, 1, 1, 'token_user1', NULL, NULL),
     
    ('user2', 'Regular', 'UserTwo', 'user2@example.com', 
     '$2a$11$gSxpmTrwPSvT1zFcfJT7Z.5HaVSZ44q9DWu8JBeYh92oGzCUxyl7m', 
     '1234567898', 'User', 1, 1, 1, 'token_user2', NULL, NULL),
     
    ('user3', 'Regular', 'UserThree', 'user3@example.com', 
     '$2a$11$gSxpmTrwPSvT1zFcfJT7Z.5HaVSZ44q9DWu8JBeYh92oGzCUxyl7m', 
     '1234567899', 'User', 1, 1, 1, 'token_user3', NULL, NULL);

-- ================================================
-- Insert Recipes
-- ================================================
INSERT INTO Recipes (
    Title, 
    Description, 
    Ingredients, 
    Steps, 
    Category, 
    ImageUrl, 
    CreatedBy, 
    CreatedAt, 
    UpdatedAt
)
VALUES
    ('Masala Dosa', 'Crispy rice pancake filled with spiced potatoes.', 
     'Rice, Urad Dal, Potato, Onion, Green Chili, Ginger, Spices', 
     '1. Soak rice and urad dal overnight.\n2. Grind into batter.\n3. Ferment batter for 8 hours.\n4. Spread batter on skillet.\n5. Fill with potato mixture.', 
     'Breakfast', 'placeholder_url/masala_dosa.jpg', 'chef1', '2023-11-15', '2023-12-01'),
     
    ('Butter Chicken', 'Creamy tomato-based curry with tender chicken pieces.', 
     'Chicken, Butter, Cream, Tomato Puree, Garlic, Ginger, Spices', 
     '1. Marinate chicken with spices.\n2. Cook chicken until tender.\n3. Prepare tomato-based gravy.\n4. Combine chicken with gravy and cream.', 
     'Lunch', 'placeholder_url/butter_chicken.jpg', 'chef1', '2023-10-20', '2023-11-05'),
     
    ('Paneer Tikka', 'Marinated paneer grilled to perfection.', 
     'Paneer, Yogurt, Spices, Capsicum, Onion, Lemon Juice', 
     '1. Marinate paneer with yogurt and spices.\n2. Skewer with vegetables.\n3. Grill or bake until charred.', 
     'Snack', 'placeholder_url/paneer_tikka.jpg', 'chef2', '2023-09-10', '2023-09-15'),
     
    ('Mango Lassi', 'Sweet yogurt-based mango drink.', 
     'Mango, Yogurt, Sugar, Cardamom, Ice', 
     '1. Blend all ingredients until smooth.\n2. Serve chilled with ice.', 
     'Drink', 'placeholder_url/mango_lassi.jpg', 'chef1', '2023-08-05', '2023-08-10'),
     
    ('Pav Bhaji', 'Spiced mashed vegetables served with buttered bread.', 
     'Potato, Tomato, Capsicum, Butter, Spices, Pav', 
     '1. Cook vegetables until soft.\n2. Mash and mix with spices.\n3. Serve with buttered pav.', 
     'Dinner', 'placeholder_url/pav_bhaji.jpg', 'chef2', '2023-07-22', '2023-07-30'),
     
    ('Vegan Tofu Curry', 'Spicy and creamy tofu curry.', 
     'Tofu, Coconut Milk, Tomato, Spices, Spinach', 
     '1. Sauté spices.\n2. Add tofu and cook.\n3. Pour coconut milk and simmer.\n4. Add spinach before serving.', 
     'Lunch', 'placeholder_url/vegan_tofu_curry.jpg', 'chef2', '2023-06-18', '2023-07-01'),
     
    ('Avocado Toast', 'Healthy avocado spread on toasted bread.', 
     'Bread, Avocado, Lemon Juice, Salt, Pepper, Chili Flakes', 
     '1. Toast the bread.\n2. Mash avocado with lemon juice, salt, and pepper.\n3. Spread on toast and garnish with chili flakes.', 
     'Breakfast', 'placeholder_url/avocado_toast.jpg', 'chef1', '2023-05-10', '2023-05-15'),
     
    ('Chole Bhature', 'Spicy chickpea curry served with fried bread.', 
     'Chickpeas, Onion, Tomato, Spices, Flour, Yogurt', 
     '1. Cook chickpeas with spices.\n2. Prepare bhature dough.\n3. Fry bhature until golden.', 
     'Dinner', 'placeholder_url/chole_bhature.jpg', 'chef2', '2023-04-25', '2023-05-05'),
     
    ('Idli Sambar', 'Steamed rice cakes served with lentil soup.', 
     'Rice, Urad Dal, Lentils, Vegetables, Spices', 
     '1. Soak and grind rice and urad dal.\n2. Ferment batter overnight.\n3. Steam idlis.\n4. Prepare sambar with lentils and vegetables.', 
     'Breakfast', 'placeholder_url/idli_sambar.jpg', 'chef1', '2023-03-12', '2023-03-20'),
     
    ('Biryani', 'Aromatic spiced rice with marinated meat.', 
     'Basmati Rice, Chicken, Yogurt, Onions, Spices, Saffron', 
     '1. Marinate chicken with yogurt and spices.\n2. Cook rice with saffron.\n3. Layer chicken and rice.\n4. Cook on low heat to blend flavors.', 
     'Lunch', 'placeholder_url/biryani.jpg', 'chef2', '2023-02-28', '2023-03-01'),
     
    ('Fruit Salad', 'Fresh mixed fruits with a tangy dressing.', 
     'Apple, Banana, Mango, Pomegranate, Lemon Juice, Honey', 
     '1. Chop all fruits.\n2. Mix lemon juice and honey for dressing.\n3. Combine and serve chilled.', 
     'Snack', 'placeholder_url/fruit_salad.jpg', 'chef1', '2023-01-15', '2023-01-20'),
     
    ('Falooda', 'Cold dessert with milk, noodles, and sweet toppings.', 
     'Milk, Falooda Sev, Basil Seeds, Rose Syrup, Ice Cream', 
     '1. Boil falooda sev until soft.\n2. Mix milk with rose syrup.\n3. Layer sev, milk, basil seeds, and ice cream.', 
     'Drink', 'placeholder_url/falooda.jpg', 'chef2', '2022-12-05', '2022-12-10'),
     
    ('Aloo Paratha', 'Stuffed flatbread with spiced potato filling.', 
     'Whole Wheat Flour, Potato, Onion, Spices, Ghee', 
     '1. Prepare dough with flour and water.\n2. Make potato filling with spices.\n3. Stuff and roll parathas.\n4. Cook on skillet with ghee.', 
     'Breakfast', 'placeholder_url/aloo_paratha.jpg', 'chef1', '2022-11-20', '2022-12-01'),
     
    ('Pani Puri', 'Crispy puris filled with spicy tamarind water and fillings.', 
     'Semolina, Potato, Chickpeas, Tamarind, Spices, Mint', 
     '1. Prepare and fry puris.\n2. Mix fillings with spices.\n3. Serve with tamarind water and mint.', 
     'Snack', 'placeholder_url/pani_puri.jpg', 'chef2', '2022-10-10', '2022-10-15'),
     
    ('Dal Makhani', 'Rich and creamy black lentil curry.', 
     'Black Lentils, Kidney Beans, Cream, Butter, Spices', 
     '1. Soak lentils overnight.\n2. Cook lentils with spices.\n3. Add butter and cream.', 
     'Dinner', 'placeholder_url/dal_makhani.jpg', 'chef1', '2022-09-05', '2022-09-10'),
     
    ('Samosa', 'Deep-fried pastry with spiced potato filling.', 
     'Flour, Potato, Peas, Spices, Oil', 
     '1. Prepare dough with flour and oil.\n2. Make potato and peas filling with spices.\n3. Fill and fold samosas.\n4. Deep fry until golden.', 
     'Snack', 'placeholder_url/samosa.jpg', 'chef2', '2022-08-18', '2022-08-25'),
     
    ('Rasam', 'Spicy South Indian soup.', 
     'Tamarind, Tomato, Lentils, Spices, Curry Leaves', 
     '1. Cook lentils until soft.\n2. Prepare tamarind water.\n3. Add tomatoes and spices.\n4. Combine with lentils and simmer.', 
     'Lunch', 'placeholder_url/rasam.jpg', 'chef1', '2022-07-12', '2022-07-20'),
     
    ('Gulab Jamun', 'Sweet deep-fried dough balls soaked in syrup.', 
     'Milk Powder, Flour, Ghee, Sugar, Rose Water', 
     '1. Mix milk powder, flour, and ghee.\n2. Shape into balls.\n3. Deep fry until golden.\n4. Soak in sugar syrup with rose water.', 
     'Dessert', 'placeholder_url/gulab_jamun.jpg', 'chef2', '2022-06-25', '2022-07-01'),
     
    ('Pesto Pasta', 'Italian pasta with Indian twist pesto sauce.', 
     'Pasta, Basil, Cilantro, Garlic, Nuts, Olive Oil, Spices', 
     '1. Cook pasta until al dente.\n2. Blend basil, cilantro, garlic, nuts, and spices with olive oil.\n3. Toss pasta with pesto sauce.', 
     'Lunch', 'placeholder_url/pesto_pasta.jpg', 'chef1', '2022-05-15', '2022-05-20'),
     
    ('Chocolate Brownie', 'Rich and fudgy chocolate brownies.', 
     'Chocolate, Butter, Sugar, Eggs, Flour, Nuts', 
     '1. Melt chocolate and butter.\n2. Mix in sugar and eggs.\n3. Add flour and nuts.\n4. Bake until set.', 
     'Dessert', 'placeholder_url/chocolate_brownie.jpg', 'chef2', '2022-04-10', '2022-04-15');

-- ================================================
-- Insert Nutritions
-- ================================================
INSERT INTO Nutritions (
    RecipeId, 
    Calories, 
    Protein, 
    Carbs, 
    Fat, 
    Vitamins
)
VALUES
    (1, 180, 6.0, 30.0, 5.0, 'Vitamin B1, B2, C'),
    (2, 300, 25.0, 10.0, 20.0, 'Vitamin A, C'),
    (3, 250, 15.0, 10.0, 15.0, 'Calcium, Vitamin D'),
    (4, 150, 4.0, 25.0, 2.0, 'Vitamin A, C'),
    (5, 350, 8.0, 40.0, 10.0, 'Vitamin A, C, B6'),
    (6, 220, 10.0, 20.0, 12.0, 'Vitamin C, E'),
    (7, 200, 5.0, 30.0, 8.0, 'Vitamin E, K'),
    (8, 400, 18.0, 50.0, 20.0, 'Vitamin B6, C'),
    (9, 170, 7.0, 28.0, 4.0, 'Vitamin A, C'),
    (10, 500, 22.0, 60.0, 18.0, 'Vitamin B12, D'),
    (11, 120, 3.0, 25.0, 2.0, 'Vitamin C'),
    (12, 250, 6.0, 35.0, 10.0, 'Vitamin B5, B6'),
    (13, 300, 9.0, 40.0, 15.0, 'Vitamin C, K'),
    (14, 180, 5.0, 25.0, 8.0, 'Vitamin E, B3'),
    (15, 400, 12.0, 45.0, 18.0, 'Vitamin A, C, B6'),
    (16, 150, 4.0, 20.0, 6.0, 'Vitamin C'),
    (17, 320, 8.0, 50.0, 12.0, 'Vitamin B12, E'),
    (18, 280, 7.0, 40.0, 14.0, 'Vitamin D, Calcium'),
    (19, 200, 5.0, 35.0, 5.0, 'Vitamin C'),
    (20, 350, 10.0, 45.0, 15.0, 'Vitamin B6, E');

-- ================================================
-- Insert MealPlans
-- ================================================
INSERT INTO MealPlans (
    Name, 
    CreatedBy, 
    StartDate, 
    EndDate
)
VALUES
    ('Weekday Healthy Plan', 'mealplanner1', '2023-11-01', '2023-11-07'),
    ('Weekend Indulgence Plan', 'mealplanner1', '2023-11-08', '2023-11-09'),
    ('Vegan Delight Plan', 'mealplanner2', '2023-11-10', '2023-11-16'),
    ('Protein Power Plan', 'mealplanner2', '2023-11-17', '2023-11-23'),
    ('Detox Plan', 'mealplanner1', '2023-11-24', '2023-11-30'),
    ('Festive Feast Plan', 'mealplanner2', '2023-12-01', '2023-12-07'),
    ('Quick Meals Plan', 'mealplanner1', '2023-12-08', '2023-12-14'),
    ('Family Favorites Plan', 'mealplanner2', '2023-12-15', '2023-12-21'),
    ('Low Carb Plan', 'mealplanner1', '2023-12-22', '2023-12-28'),
    ('Summer Coolers Plan', 'mealplanner2', '2023-12-29', '2024-01-04');


-- ================================================
-- Insert MealPlanRecipes
-- ================================================
INSERT INTO MealPlanRecipes (
    MealPlanId, 
    RecipeId, 
    MealPlanRecipeId, 
    MealTime
)
VALUES
    -- Weekday Healthy Plan (MealPlanId = 1)
    (1, 1, 1001, '2024-12-04T08:00:00'), -- Breakfast
    (1, 4, 1002, '2024-12-04T10:00:00'), -- Snack
    (1, 5, 1003, '2024-12-04T19:00:00'), -- Dinner
    (1, 7, 1004, '2024-12-04T13:00:00'), -- Lunch
    (1, 9, 1005, '2024-12-04T20:00:00'), -- Dinner
    
    -- Weekend Indulgence Plan (MealPlanId = 2)
    (2, 2, 2001, '2024-12-05T13:00:00'), -- Lunch
    (2, 3, 2002, '2024-12-05T10:00:00'), -- Snack
    (2, 10, 2003, '2024-12-05T13:00:00'), -- Lunch
    (2, 14, 2004, '2024-12-05T21:00:00'), -- Dessert
    (2, 17, 2005, '2024-12-05T13:00:00'), -- Lunch
    
    -- Vegan Delight Plan (MealPlanId = 3)
    (3, 6, 3001, '2024-12-06T13:00:00'), -- Lunch
    (3, 7, 3002, '2024-12-06T08:00:00'), -- Breakfast
    (3, 16, 3003, '2024-12-06T13:00:00'), -- Lunch
    (3, 18, 3004, '2024-12-06T19:00:00'), -- Dinner
    (3, 19, 3005, '2024-12-06T10:00:00'), -- Snack
    
    -- Protein Power Plan (MealPlanId = 4)
    (4, 2, 4001, '2024-12-07T13:00:00'), -- Lunch
    (4, 10, 4002, '2024-12-07T13:00:00'), -- Lunch
    (4, 17, 4003, '2024-12-07T13:00:00'), -- Lunch
    (4, 20, 4004, '2024-12-07T13:00:00'), -- Lunch
    (4, 5, 4005, '2024-12-07T19:00:00'), -- Dinner
    
    -- Detox Plan (MealPlanId = 5)
    (5, 11, 5001, '2024-12-08T10:00:00'), -- Snack
    (5, 12, 5002, '2024-12-08T13:00:00'), -- Lunch
    (5, 13, 5003, '2024-12-08T13:00:00'), -- Lunch
    (5, 19, 5004, '2024-12-08T10:00:00'), -- Snack
    (5, 9, 5005, '2024-12-08T19:00:00'), -- Dinner
    
    -- Festive Feast Plan (MealPlanId = 6)
    (6, 14, 6001, '2024-12-09T21:00:00'), -- Dessert
    (6, 15, 6002, '2024-12-09T19:00:00'), -- Dinner
    (6, 16, 6003, '2024-12-09T13:00:00'), -- Lunch
    (6, 17, 6004, '2024-12-09T13:00:00'), -- Lunch
    (6, 20, 6005, '2024-12-09T13:00:00'), -- Lunch
    
    -- Quick Meals Plan (MealPlanId = 7)
    (7, 7, 7001, '2024-12-10T08:00:00'), -- Breakfast
    (7, 8, 7002, '2024-12-10T13:00:00'), -- Lunch
    (7, 11, 7003, '2024-12-10T10:00:00'), -- Snack
    (7, 13, 7004, '2024-12-10T13:00:00'), -- Lunch
    (7, 19, 7005, '2024-12-10T10:00:00'), -- Snack
    
    -- Family Favorites Plan (MealPlanId = 8)
    (8, 3, 8001, '2024-12-11T10:00:00'), -- Snack
    (8, 4, 8002, '2024-12-11T10:00:00'), -- Snack
    (8, 5, 8003, '2024-12-11T19:00:00'), -- Dinner
    (8, 6, 8004, '2024-12-11T13:00:00'), -- Lunch
    (8, 12, 8005, '2024-12-11T13:00:00'), -- Lunch
    
    -- Low Carb Plan (MealPlanId = 9)
    (9, 2, 9001, '2024-12-12T13:00:00'), -- Lunch
    (9, 3, 9002, '2024-12-12T10:00:00'), -- Snack
    (9, 6, 9003, '2024-12-12T13:00:00'), -- Lunch
    (9, 8, 9004, '2024-12-12T13:00:00'), -- Lunch
    (9, 10, 9005, '2024-12-12T13:00:00'), -- Lunch
    
    -- Summer Coolers Plan (MealPlanId = 10)
    (10, 4, 10001, '2024-12-13T10:00:00'), -- Snack
    (10, 12, 10002, '2024-12-13T13:00:00'), -- Lunch
    (10, 14, 10003, '2024-12-13T21:00:00'), -- Dessert
    (10, 16, 10004, '2024-12-13T13:00:00'), -- Lunch
    (10, 18, 10005, '2024-12-13T19:00:00'); -- Dinner


-- ================================================
-- Insert Reviews
-- ================================================
INSERT INTO Reviews (
    RecipeId, 
    UserName, 
    Rating, 
    Comment, 
    CreatedAt
)
VALUES
    -- Reviews for Masala Dosa (RecipeId = 1)
    (1, 'user1', 5, 'Crispy and flavorful!', '2023-11-16'),
    (1, 'user2', 4, 'Loved the spice level.', '2023-11-17'),
    (1, 'user3', 5, 'Perfect dosa every time!', '2023-11-18'),
    
    -- Reviews for Butter Chicken (RecipeId = 2)
    (2, 'user1', 5, 'Creamy and delicious!', '2023-10-21'),
    (2, 'user2', 4, 'Rich flavor, could be spicier.', '2023-10-22'),
    (2, 'user3', 5, 'Best butter chicken ever!', '2023-10-23'),
    
    -- Reviews for Paneer Tikka (RecipeId = 3)
    (3, 'user1', 4, 'Great taste and texture.', '2023-09-11'),
    (3, 'user2', 5, 'Absolutely loved it!', '2023-09-12'),
    (3, 'user3', 5, 'Perfectly grilled and spiced.', '2023-09-13'),
    
    -- Reviews for Mango Lassi (RecipeId = 4)
    (4, 'user1', 5, 'So refreshing!', '2023-08-06'),
    (4, 'user2', 4, 'Sweet and tangy.', '2023-08-07'),
    (4, 'user3', 5, 'My favorite drink!', '2023-08-08'),
    
    -- Reviews for Pav Bhaji (RecipeId = 5)
    (5, 'user1', 4, 'Spicy and tasty.', '2023-07-23'),
    (5, 'user2', 5, 'Delicious with buttered pav!', '2023-07-24'),
    (5, 'user3', 5, 'A family favorite!', '2023-07-25'),
    
    -- Reviews for Vegan Tofu Curry (RecipeId = 6)
    (6, 'user1', 5, 'Perfect for vegan diet.', '2023-06-19'),
    (6, 'user2', 4, 'Good flavor but needs more spice.', '2023-06-20'),
    (6, 'user3', 5, 'Loved the creamy texture!', '2023-06-21'),
    
    -- Reviews for Avocado Toast (RecipeId = 7)
    (7, 'user1', 5, 'Healthy and delicious!', '2023-05-11'),
    (7, 'user2', 4, 'Great start to the day.', '2023-05-12'),
    (7, 'user3', 5, 'Perfectly seasoned.', '2023-05-13'),
    
    -- Reviews for Chole Bhature (RecipeId = 8)
    (8, 'user1', 4, 'Spicy and filling.', '2023-04-26'),
    (8, 'user2', 5, 'Authentic taste!', '2023-04-27'),
    (8, 'user3', 5, 'Loved every bite!', '2023-04-28'),
    
    -- Reviews for Idli Sambar (RecipeId = 9)
    (9, 'user1', 5, 'Soft idlis with flavorful sambar.', '2023-03-13'),
    (9, 'user2', 4, 'Good, but sambar could be thicker.', '2023-03-14'),
    (9, 'user3', 5, 'Perfect breakfast!', '2023-03-15'),
    
    -- Reviews for Biryani (RecipeId = 10)
    (10, 'user1', 5, 'Aromatic and tasty!', '2023-02-28'),
    (10, 'user2', 4, 'Good flavor.', '2023-03-01'),
    (10, 'user3', 5, 'Best biryani I have had!', '2023-03-02'),
    
    -- Reviews for Fruit Salad (RecipeId = 11)
    (11, 'user1', 5, 'Fresh and healthy!', '2023-01-16'),
    (11, 'user2', 4, 'Very refreshing.', '2023-01-17'),
    (11, 'user3', 5, 'Perfect mix of fruits.', '2023-01-18'),
    
    -- Reviews for Falooda (RecipeId = 12)
    (12, 'user1', 5, 'Sweet and delicious!', '2022-12-06'),
    (12, 'user2', 4, 'Great dessert.', '2022-12-07'),
    (12, 'user3', 5, 'Loved the texture!', '2022-12-08'),
    
    -- Reviews for Aloo Paratha (RecipeId = 13)
    (13, 'user1', 4, 'Good but a bit oily.', '2022-11-16'),
    (13, 'user2', 5, 'Perfectly spiced filling.', '2022-11-17'),
    (13, 'user3', 5, 'Best paratha ever!', '2022-11-18'),
    
    -- Reviews for Pani Puri (RecipeId = 14)
    (14, 'user1', 5, 'Crispy and tangy!', '2022-10-11'),
    (14, 'user2', 4, 'Great street food!', '2022-10-12'),
    (14, 'user3', 5, 'Loved the spicy water.', '2022-10-13'),
    
    -- Reviews for Dal Makhani (RecipeId = 15)
    (15, 'user1', 5, 'Rich and creamy!', '2022-09-06'),
    (15, 'user2', 4, 'Good flavor.', '2022-09-07'),
    (15, 'user3', 5, 'Perfect accompaniment to rice.', '2022-09-08'),
    
    -- Reviews for Samosa (RecipeId = 16)
    (16, 'user1', 4, 'Crispy and tasty.', '2022-08-19'),
    (16, 'user2', 5, 'Loved the filling.', '2022-08-20'),
    (16, 'user3', 5, 'Perfect snack!', '2022-08-21'),
    
    -- Reviews for Rasam (RecipeId = 17)
    (17, 'user1', 5, 'Spicy and tangy!', '2022-07-13'),
    (17, 'user2', 4, 'Good for digestion.', '2022-07-14'),
    (17, 'user3', 5, 'Perfect with rice.', '2022-07-15'),
    
    -- Reviews for Gulab Jamun (RecipeId = 18)
    (18, 'user1', 5, 'Sweet and soft!', '2022-06-26'),
    (18, 'user2', 4, 'Great dessert.', '2022-06-27'),
    (18, 'user3', 5, 'Loved the syrup.', '2022-06-28'),
    
    -- Reviews for Pesto Pasta (RecipeId = 19)
    (19, 'user1', 4, 'Good fusion dish.', '2022-05-16'),
    (19, 'user2', 5, 'Loved the flavors.', '2022-05-17'),
    (19, 'user3', 5, 'Perfect blend of herbs.', '2022-05-18'),
    
    -- Reviews for Chocolate Brownie (RecipeId = 20)
    (20, 'user1', 5, 'Rich and fudgy!', '2022-04-11'),
    (20, 'user2', 4, 'Good but a bit sweet.', '2022-04-12'),
    (20, 'user3', 5, 'Perfect dessert!', '2022-04-13');



-- ================================================
-- Insert ShoppingLists
-- ================================================
INSERT INTO ShoppingLists (
    MealPlanId
)
VALUES
    (1), -- Weekday Healthy Plan
    (2), -- Weekend Indulgence Plan
    (3), -- Vegan Delight Plan
    (4), -- Protein Power Plan
    (5), -- Detox Plan
    (6), -- Festive Feast Plan
    (7), -- Quick Meals Plan
    (8), -- Family Favorites Plan
    (9), -- Low Carb Plan
    (10); -- Summer Coolers Plan

-- ================================================
-- Insert ShoppingListItems
-- ================================================
INSERT INTO ShoppingListItems (
    ShoppingListId, 
    Ingredient, 
    Quantity, 
    IsPurchased
)
VALUES
    -- Shopping List 1 (Weekday Healthy Plan)
    (1, 'Rice', 2, 0),
    (1, 'Urad Dal', 1, 0),
    (1, 'Potato', 5, 0),
    (1, 'Onion', 3, 0),
    (1, 'Green Chili', 10, 0),
    (1, 'Ginger', 1, 0),
    (1, 'Spices', 1, 0),
    (1, 'Butter', 1, 0),
    (1, 'Cream', 1, 0),
    (1, 'Paneer', 2, 0),
    
    -- Shopping List 2 (Weekend Indulgence Plan)
    (2, 'Chicken', 4, 0),
    (2, 'Butter', 1, 0),
    (2, 'Tomato Puree', 2, 0),
    (2, 'Garlic', 5, 0),
    (2, 'Ginger', 2, 0),
    (2, 'Yogurt', 3, 0),
    (2, 'Capsicum', 4, 0),
    (2, 'Lemon Juice', 1, 0),
    (2, 'Pasta', 2, 0),
    (2, 'Chocolate', 3, 0),
    
    -- Shopping List 3 (Vegan Delight Plan)
    (3, 'Tofu', 2, 0),
    (3, 'Coconut Milk', 2, 0),
    (3, 'Spinach', 1, 0),
    (3, 'Basil', 1, 0),
    (3, 'Cilantro', 1, 0),
    (3, 'Lentils', 2, 0),
    (3, 'Tamarind', 1, 0),
    (3, 'Sugar', 1, 0),
    (3, 'Honey', 1, 0),
    (3, 'Rose Syrup', 1, 0),
    
    -- Shopping List 4 (Protein Power Plan)
    (4, 'Chicken', 5, 0),
    (4, 'Butter', 2, 0),
    (4, 'Tomato Puree', 3, 0),
    (4, 'Cream', 2, 0),
    (4, 'Yogurt', 4, 0),
    (4, 'Spices', 1, 0),
    (4, 'Eggs', 12, 0),
    (4, 'Cheese', 2, 0),
    (4, 'Basil', 1, 0),
    (4, 'Olive Oil', 1, 0),
    
    -- Shopping List 5 (Detox Plan)
    (5, 'Apple', 6, 0),
    (5, 'Banana', 8, 0),
    (5, 'Mango', 4, 0),
    (5, 'Pomegranate', 3, 0),
    (5, 'Lemon Juice', 2, 0),
    (5, 'Honey', 2, 0),
    (5, 'Spinach', 2, 0),
    (5, 'Coconut Water', 1, 0),
    (5, 'Carrots', 5, 0),
    (5, 'Cucumber', 4, 0),
    
    -- Shopping List 6 (Festive Feast Plan)
    (6, 'Ghee', 2, 0),
    (6, 'Milk Powder', 1, 0),
    (6, 'Flour', 5, 0),
    (6, 'Sugar', 3, 0),
    (6, 'Rose Water', 1, 0),
    (6, 'Paneer', 3, 0),
    (6, 'Basil', 1, 0),
    (6, 'Cilantro', 1, 0),
    (6, 'Spices', 2, 0),
    (6, 'Eggs', 6, 0),
    
    -- Shopping List 7 (Quick Meals Plan)
    (7, 'Avocado', 4, 0),
    (7, 'Whole Wheat Bread', 2, 0),
    (7, 'Lemon Juice', 1, 0),
    (7, 'Salt', 1, 0),
    (7, 'Pepper', 1, 0),
    (7, 'Chili Flakes', 1, 0),
    (7, 'Pasta', 3, 0),
    (7, 'Basil', 1, 0),
    (7, 'Cilantro', 1, 0),
    (7, 'Nuts', 1, 0),
    
    -- Shopping List 8 (Family Favorites Plan)
    (8, 'Paneer', 3, 0),
    (8, 'Yogurt', 2, 0),
    (8, 'Capsicum', 3, 0),
    (8, 'Onion', 4, 0),
    (8, 'Ginger', 2, 0),
    (8, 'Garlic', 4, 0),
    (8, 'Tomatoes', 5, 0),
    (8, 'Rice', 3, 0),
    (8, 'Semolina', 2, 0),
    (8, 'Chickpeas', 2, 0),
    
    -- Shopping List 9 (Low Carb Plan)
    (9, 'Chicken', 6, 0),
    (9, 'Butter', 2, 0),
    (9, 'Tomato Puree', 3, 0),
    (9, 'Cream', 3, 0),
    (9, 'Yogurt', 4, 0),
    (9, 'Spices', 2, 0),
    (9, 'Eggs', 12, 0),
    (9, 'Cheese', 3, 0),
    (9, 'Basil', 1, 0),
    (9, 'Olive Oil', 2, 0),
    
    -- Shopping List 10 (Summer Coolers Plan)
    (10, 'Mango', 5, 0),
    (10, 'Yogurt', 3, 0),
    (10, 'Sugar', 2, 0),
    (10, 'Cardamom', 1, 0),
    (10, 'Ice', 10, 0),
    (10, 'Falooda Sev', 2, 0),
    (10, 'Basil Seeds', 1, 0),
    (10, 'Rose Syrup', 1, 0),
    (10, 'Ice Cream', 3, 0),
    (10, 'Chocolate', 2, 0);


-- ================================================
-- Insert ShoppingLists
-- ================================================
-- (Already inserted above)

-- ================================================
-- Insert ShoppingListItems
-- ================================================
-- (Already inserted above)

-- ================================================
-- Note:
-- 1. **PasswordHash**:
--    - The provided BCrypt hashes are **sample placeholders**. Replace them with actual BCrypt hashes generated by your application.
--    - Example format: '$2a$10$EixZaYVK1fsbw1ZfbX3OXePaWxn96p36V1OiYN6jCkP/5gWjQvCmi'
-- 
-- 2. **Image URLs**:
--    - Replace `'placeholder_url/your_image.jpg'` with actual image URLs where your images are hosted.
-- 
-- 3. **Date Formats**:
--    - Ensure that all dates are in the `'YYYY-MM-DD'` format and are valid.
-- 
-- 4. **Foreign Keys**:
--    - Ensure that all foreign key references (`CreatedBy`, `RecipeId`, `MealPlanId`, etc.) correspond correctly to existing entries in their respective tables.
-- 
-- 5. **Additional Fields**:
--    - If your `Users` table has additional fields like `PasswordResetToken` and `PasswordResetTokenExpiry`, they are included in the `INSERT` statements and set to `NULL` where applicable.
-- 
-- 6. **Data Volume**:
--    - This script inserts substantial data to thoroughly test your backend. Adjust the number of entries as needed based on your testing requirements.
-- 
-- 7. **Running the Script**:
--    - **Backup Existing Data**: If you're populating an existing database, ensure you have backups to prevent data loss.
--    - **Execute in Order**: Run the script in the order provided to maintain referential integrity.
--    - **Verify Inserts**: After running the script, perform `SELECT` queries on each table to verify that data has been inserted correctly.
--    - **Test Backend Functionality**: Use your backend API or application to interact with the data, ensuring all CRUD operations work as expected.
