import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

void main() {
  runApp(const WarehouseApp());
}

class WarehouseApp extends StatelessWidget {
  const WarehouseApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Warehouse',
      theme: ThemeData(
        colorSchemeSeed: Colors.blue,
        useMaterial3: true,
      ),
      home: const ProductListPage(),
    );
  }
}

class Product {
  final int id;
  final String name;
  final String location;

  const Product({
    required this.id,
    required this.name,
    required this.location,
  });

  factory Product.fromJson(Map<String, dynamic> json) {
    // Factory is used to create an object that requires a conversion step
    return Product(
        id: json['id'] as int,
        name: json['name'] as String,
        location: json['location'] as String);
  }
}

class ProductListPage extends StatefulWidget {
  const ProductListPage({super.key});

  @override
  State<ProductListPage> createState() => _ProductListPageState();
}

class _ProductListPageState extends State<ProductListPage> {
  static const apiUrl = 'http://localhost:5080/api/products';

  late Future<List<Product>> products;

  @override
  void initState() {
    super.initState();
    products = fetchProducts();
  }

  Future<List<Product>> fetchProducts() async {
    final response = await http.get(Uri.parse(apiUrl));

    if (response.statusCode != 200) {
      throw Exception(
        'API returned status ${response.statusCode}',
      );
    }

    final List<dynamic> jsonList = jsonDecode(response.body) as List<dynamic>;

    return jsonList
        .map((json) => Product.fromJson(
              json as Map<String, dynamic>,
            ))
        .toList();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Warehouse products'),
      ),
      body: FutureBuilder<List<Product>>(
        future: products,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(
              child: CircularProgressIndicator(),
            );
          }

          if (snapshot.hasError) {
            return Center(
              child: Padding(
                padding: const EdgeInsets.all(16),
                child: Text('Could not load Products:\n${snapshot.error}'),
              ),
            );
          }

          final items = snapshot.data ?? [];

          if (items.isEmpty) {
            return const Center(
              child: Text('No products found.'),
            );
          }

          return ListView.builder(
              itemCount: items.length,
              itemBuilder: (context, index) {
                final product = items[index];

                return ListTile(
                    leading: const Icon(Icons.inventory_2),
                    title: Text(product.name),
                    subtitle: Text(
                      'Location: ${product.location}',
                    ),
                    trailing: Text('#${product.id}'));
              });
        },
      ),
    );
  }
}
